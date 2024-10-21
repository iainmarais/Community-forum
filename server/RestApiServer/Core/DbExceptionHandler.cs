using RestApiServer.Core.ApiCoreResponses;
using Serilog;

public class DbExceptionHandler : IDbExceptionHandler
{
    private readonly RequestDelegate _next;
    public ApiCoreErrorResponse ApiCoreErrorResponse { get; set; } = new();

    // Constructor to accept RequestDelegate
    public DbExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

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
