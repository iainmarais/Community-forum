using RestApiServer.Core.ApiResponses;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Hubs;
using RestApiServer.Utils;
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

            var app = builder.Build();

            string productionCorsPolicy = "productionCorsPolicy";
            string developmentCorsPolicy = "developmentCorsPolicy";

            if (app.Environment.IsDevelopment())
            {
                app.UseCors(developmentCorsPolicy);
            }
            else
            {
                app.UseCors(productionCorsPolicy);
            }
            
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
                ApiErrorResponse? errorResponse = null;
                try
                {
                    await next(context);
                    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                    {
                        errorResponse = new ApiErrorResponse
                        {
                            ResponseMessage = "Not found"
                        };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                    {
                        errorResponse = new ApiErrorResponse
                        {
                            ResponseMessage = "Unauthorized"
                        };
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                    {
                        errorResponse = new ApiErrorResponse
                        {
                            ResponseMessage = "You are not allowed to perform this action"
                        };

                        Log.Information($"{method} {url}, User not allowed to perform action");
                        await context.Response.WriteAsJsonAsync(errorResponse);
                    }
                }
                catch (ClientInducedException ex)                
                {
                    errorResponse = new ApiClientErrorResponse()
                    {
                        ResponseMessage = ex.Message,
                        ClientErrorData = ex.ValidationErrors
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(errorResponse);
                }
                catch(UnauthorizedAccessException ex)
                {
                    errorResponse = new ApiErrorResponse
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
                    errorResponse = new ApiErrorResponse
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
                        var user = AuthUtils.GetForumUserContext(context.User);
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
            return app;
        }         
    }
}