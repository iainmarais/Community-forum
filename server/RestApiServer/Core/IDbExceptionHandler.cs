using RestApiServer.Core.ApiCoreResponses;

public interface IDbExceptionHandler
{
    public ApiCoreErrorResponse ApiCoreErrorResponse { get; set; }
}
