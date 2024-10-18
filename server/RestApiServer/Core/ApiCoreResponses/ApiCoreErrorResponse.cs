using System.Text.Json.Serialization;
//This class is used by the server core for API responses. It avoids confusion with the ApiResponses class in the RestApiServer.Endpoints class library.
namespace RestApiServer.Core.ApiCoreResponses
{
    [JsonDerivedType(typeof(ApiCoreClientErrorResponse))]
    public class ApiCoreErrorResponse
    {
        public string? ResponseMessage { get; set; }
    }

    public class ApiCoreClientErrorResponse : ApiCoreErrorResponse
    {
        public object? ClientErrorData { get; set; }
    }
}