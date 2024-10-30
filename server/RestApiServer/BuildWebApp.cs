using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApiServer.Common.Config;
using RestApiServer.Common.Services;
using RestApiServer.Core.ApiCoreResponses;
using RestApiServer.Core.Errorhandler;
using RestApiServer.SignalR.Hubs;
using Serilog;

namespace RestApiServer
{
    public partial class Server
    {
        private static WebApplication BuildWebApp(WebApplicationBuilder builder)
        {
            List<int> dontLogStatusCodes = new() { 404 };
            List<string> dontLogEndpoints =new()  {"health/info", "/api/greeting"};
            List<string> dontLogBodyEndpoints = new();
            try 
            {
                var app = builder.Build();

                string productionCorsPolicy = "productionCorsPolicy";
                string developmentCorsPolicy = "developmentCorsPolicy";

                if (app.Environment.IsDevelopment())
                {
                    app.UseCors(developmentCorsPolicy);
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseCors(productionCorsPolicy);
                    app.UseExceptionHandler($"{ConfigurationLoader.GetConfigValue(EnvironmentVariable.ServerListenLocalhostIp)}/v1/error");
                }
                
                //Use the exception handler middleware to deal with any exceptions thrown.

                app.UseStaticFiles();
                
                app.UseRouting();

                //Toplevel registration of endpoints
                app.MapControllers(); // Maps the API controllers

                //Map the SignalR hubs
                app.MapHub<ChatHub>("/v1/hubs/chat"); // Maps the SignalR hub for chat
                app.MapHub<ForumStatsHub>("/v1/hubs/forumstats"); 

                app.UseWebSockets();

                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering();

                    var method = context.Request.Method;
                    var url = context.Request.Path;
                    var requestStart = DateTime.Now;

                    string jsonPayload;
                    if(dontLogBodyEndpoints.Any(endpoint => url.ToString().Contains(endpoint)))
                    {
                        jsonPayload = "<removed>";
                    }
                    else
                    {
                        var reader = new StreamReader(context.Request.Body);
                        jsonPayload = await reader.ReadToEndAsync();
                        context.Request.Body.Seek(0, SeekOrigin.Begin);
                    }
                    ApiCoreErrorResponse? errorResponse = null;
                    try
                    {
                        await next(context);
                        if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                        {
                            errorResponse = new ApiCoreErrorResponse
                            {
                                ResponseMessage = "Not found"
                            };
                            await context.Response.WriteAsJsonAsync(errorResponse);
                        }
                        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                        {
                            if(context.Request.Headers.ContainsKey("Authorization"))
                            {
                                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                                var tokenHandler = new JwtSecurityTokenHandler();

                                //Reusing the same params from my ConfigureServices.cs file
                                var validationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = false,
                                    ValidateAudience = false,
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    ClockSkew = TimeSpan.Zero,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret)))
                                };
                                try
                                {
                                    var claimsPrinicpal  = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                                    //At this point the token is good, but the user does not have the appropriate authorisation.
                                    //Bite me, I use British English :D
                                    errorResponse = new ApiCoreErrorResponse
                                    {
                                        ResponseMessage = "Unauthorised",
                                        StatusCode = 401,
                                        StatusMessage = "You are not authorised to access this endpoint."
                                    };
                                }
                                catch (SecurityTokenExpiredException)
                                {
                                    errorResponse = new ApiCoreErrorResponse
                                    {
                                        ResponseMessage = "Unauthorised",
                                        StatusCode = 401,
                                        StatusMessage = "Your token has expired, and needs to be refreshed in order to continue."
                                    };                                    
                                }
                                catch(Exception) 
                                {
                                    errorResponse = new ApiCoreErrorResponse
                                    {
                                        ResponseMessage = "Unauthorised",
                                        StatusCode = 401,
                                        StatusMessage = "Your token is invalid, please try logging in again."
                                    };                                     
                                }

                                await context.Response.WriteAsJsonAsync(errorResponse);
                            }
                            //Other HTTP401 cases
                            else
                            {
                                errorResponse = new ApiCoreErrorResponse
                                {
                                    ResponseMessage = "Unauthorized",
                                    StatusCode = 401,
                                };
                                await context.Response.WriteAsJsonAsync(errorResponse);
                            }
                        }
                        if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                        {
                            errorResponse = new ApiCoreErrorResponse
                            {
                                ResponseMessage = "You are not allowed to perform this action"
                            };

                            Log.Information($"{method} {url}, User not allowed to perform action");
                            await context.Response.WriteAsJsonAsync(errorResponse);
                        }
                    }
                    catch (ClientInducedException ex)                
                    {
                        errorResponse = new ApiCoreClientErrorResponse()
                        {
                            ResponseMessage = ex.Message,
                            ClientErrorData = ex.ValidationErrors
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    catch(UnauthorizedAccessException ex)
                    {
                        errorResponse = new ApiCoreErrorResponse
                        {
                            ResponseMessage = ex.Message
                        };
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    catch (Exception ex)
                    {
                        
                        string message = "Something went wrong";
                        if(app.Environment.IsDevelopment())
                        {
                            message += $" {ex.Message} {ex.InnerException?.Message ?? ""}";
                        }
                        errorResponse = new ApiCoreErrorResponse
                        {
                            ResponseMessage = message
                        };
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    finally
                    {
                        var statusCode = context.Response.StatusCode;
                        var requestTime = DateTime.Now - requestStart;
                        string? exMessage = null;
                        if (errorResponse != null)
                        {
                            exMessage = errorResponse.ResponseMessage;
                        }

                        string? userIdentity = null;
                        try
                        {
                            var user = AuthService.GetForumUserContext(context.User);
                            userIdentity = "{User:" + user.UserId + "}";
                        }
                        catch(Exception)
                        {

                        }
                        if(!dontLogStatusCodes.Contains(statusCode))
                        {
                            if(!dontLogEndpoints.Any(ep => url.ToString().Contains(ep)))
                            {
                                Log.Information("{Method} {StatusCode} {Url} {RequestTime}ms {UserIdentity} {ErrorMessage}, body: {RequestBody}", method, statusCode, url, requestTime, userIdentity ?? "", exMessage ?? "", jsonPayload);
                            }
                        }
                    }
                    
                });

                app.UseAuthentication();
                app.UseAuthorization();

                app.UseMiddleware<DbExceptionHandler>();

                return app;
            }
            catch (AggregateException ex)
            {
                Log.Fatal($"Failed to build web app.\n {ex.Message}", ex);
                throw;
            }
            catch(Exception ex)
            {
                Log.Fatal($"Failed to build web app.\n {ex.Message}", ex);
                throw;
            }  
        }       
    }
}