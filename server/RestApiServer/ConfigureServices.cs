using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RestApiServer.Common.Config;
using RestApiServer.Security;

namespace RestApiServer
{
    public partial class Server
    {
        private static void ConfigureServices(IServiceCollection services)
        {
            string productionCorsPolicy = "productionCorsPolicy";
            string developmentCorsPolicy = "developmentCorsPolicy";
            
            services.AddControllers().AddJsonOptions(options =>
            {

            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
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

            services.AddAuthorization(
                async options => 
                {
                    RoleBasedAuth.Configure(options);
                    await UserAuthorisationPolicies.Configure(options);
                }
            );

            services.AddCors(options =>
            {
                options.AddPolicy(developmentCorsPolicy, policy =>
                {
                    policy.SetIsOriginAllowed(origin => IsAllowedOrigin(origin, true))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });

                options.AddPolicy(productionCorsPolicy, policy =>
                {
                    policy.SetIsOriginAllowed(origin => IsAllowedOrigin(origin, false))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });
            });           
            //Lastly, add SignalR to services.
            services.AddSignalR();
        }

        private static bool IsAllowedOrigin(string origin, bool isDevelopment)
        {
            //Allow HTTP from local
            if(origin.StartsWith("http://localhost") || origin.StartsWith("http://127.0.0.1") )
            {
               
                return true;
            }
            
            //Allow HTTPS from local
            if(origin.StartsWith("https://localhost") || origin.StartsWith("https://127.0.0.1") )
            {
                return true;
            }

            if(!isDevelopment)
            {
                var allocatedNetworkRange = "192.168.0.";
                Uri uri = new(origin);
                return uri.Host.StartsWith(allocatedNetworkRange);
            }
            //Else, return false.
            return false;
        }
    }
}