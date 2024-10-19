using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using RestApiServer.Core.ApiCoreResponses;

namespace RestApiServer.ErrorHandler
{
    public static class DbExceptionHandlerExtensions
    {
        /// <summary>
        /// Adds a default implementation for the <see cref="IDbExceptionHandler"/> service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddDbExceptionHandler(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IDbExceptionHandler, DbExceptionHandler>();
            return services;
        }
        public static IApplicationBuilder UseDbExceptionHandler(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.UseMiddleware<DbExceptionHandler>();
        }        
    }

    public class DbExceptionHandler : IDbExceptionHandler
    {
        public ApiCoreErrorResponse ApiCoreErrorResponse { get; set; } = new();

        public async Task HandleAsync(HttpContext context, Exception exception)
        {
            Log.Error(exception, "Critical error in server. Shutting down server...");

            var errorResponse = new ApiCoreErrorResponse
            {
                StatusCode = 500,
                StatusMessage = "Something went wrong",
                ExceptionType = exception.GetType().ToString(),
                ExceptionMessage = exception.Message
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
            // Shut down the server. Any non-zero exit code indicates an error
            ShutDownServerSafely();
        }

        private void ShutDownServerSafely()
        {
            // Shut down the server. Any non-zero exit code indicates an error
            Log.Information("Shutting down server due to critical error...");
            Environment.Exit(1);
        
        }    
    }
}
