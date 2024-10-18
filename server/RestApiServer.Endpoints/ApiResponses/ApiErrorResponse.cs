using System.Text.Json.Serialization;

namespace RestApiServer.Endpoints.ApiResponses
{
    [JsonDerivedType(typeof(ApiClientErrorResponse))]
    public class ApiErrorResponse
    {
        public string? ResponseMessage { get; set; }
    }

    public class ApiClientErrorResponse : ApiErrorResponse
    {
        public object? ClientErrorData { get; set; }
    }
}