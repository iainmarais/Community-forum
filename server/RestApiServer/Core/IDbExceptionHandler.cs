using Microsoft.AspNetCore.Http;
using RestApiServer.Core.ApiCoreResponses;

namespace RestApiServer.ErrorHandler
{
    public interface IDbExceptionHandler
    {
        public ApiCoreErrorResponse ApiCoreErrorResponse { get; set; }
    }
}