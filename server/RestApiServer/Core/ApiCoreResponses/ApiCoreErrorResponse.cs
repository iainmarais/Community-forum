using System.Text.Json.Serialization;
//This class is used by the server core for API responses. It avoids confusion with the ApiResponses class in the RestApiServer.Endpoints class library.
namespace RestApiServer.Core.ApiCoreResponses
{
    [JsonDerivedType(typeof(ApiCoreClientErrorResponse))]
    public class ApiCoreErrorResponse
    {
        public int StatusCode { get; set; } = 0;
        public string StatusMessage { get; set; } = "";
        public string ExceptionType { get; set; } = "";
        public string ExceptionMessage { get; set; } = "";
        public string? ResponseMessage { get; set; }
    }

    public class ApiCoreClientErrorResponse : ApiCoreErrorResponse
    {
        public object? ClientErrorData { get; set; }
    }
}