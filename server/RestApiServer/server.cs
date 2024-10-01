using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Text;
using RestApiServer.Core.Config;
using RestApiServer.Core.Security;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Utils;
using RestApiServer.Db;
using Serilog;
using RestApiServer.Hubs;


namespace RestApiServer
{
    public class Server
    {
        public static void StartServer(string[] args)
        {
            Log.Logger  = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-{Date}.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up... Please be patient.");
                var builder = WebApplication.CreateBuilder(args);

                //Use serilog
                builder.Host.UseSerilog();

                EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
                Encoding.RegisterProvider(encodingProvider);
                ConfigurationLoader.LoadConfig();
                
                List<int> dontLogStatusCodes = [ 404 ];
                List<string> dontLogEndpoints = ["/health/info", "/api/greeting"];
                List<string> dontLogBodyEndpoints = [];

                builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    //Todo: Build out if necessary.
                });

                //Add SignalR to services
                builder.Services.AddSignalR();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddAuthentication(options=> 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret)))
                    };
                });
                builder.Services.AddAuthorization(
                    options => 
                    {
                        UserAuthorisationPolicies.Configure(options);
                    }
                );

                string productionCorsPolicy = "productionCorsPolicy";

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(productionCorsPolicy, policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition");
                    });
                });

                string developmentCorsPolicy = "developmentCorsPolicy";
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(developmentCorsPolicy, policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition");
                    });
                });

                var app = builder.Build();

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
                            //SystemInsights.ReportError($"{method} {url}, User not allowed to perform action");
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

                if(args.Contains("help"))
                {
                    //List the args available and exit.
                    Console.WriteLine("setup-database-only: Sets up the database only and exits");
                    Console.WriteLine("setup-database: Sets up the database and starts the server");
                    return;
                }                       

                app.Run();
            }

            catch (Exception ex)
            {
                Log.Fatal(ex, "Server startup failed.");
                throw;
            }

            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}