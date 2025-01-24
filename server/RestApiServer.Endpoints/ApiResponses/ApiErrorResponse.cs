using System.Text.Json.Serialization;

namespace RestApiServer.Endpoints.ApiResponses
{
    //API client error response with data
    public class ApiErrorResponse<D> : ApiResponse<D>
    {
        public ApiErrorResponse(string errorMessage, D? data) : base(errorMessage, data)
        {
            Message = errorMessage;
            Data = data;
        }
    }

    public class ApiErrorResponseWithMetadata<D, M> : ApiErrorResponse<D>
    {
        public M? Metadata { get; set; }
        public ApiErrorResponseWithMetadata(string errorMessage, D? data, M? metadata) : base(errorMessage, data) 
        { 
            Metadata = metadata; 
        }
    }

    public static class ApiClientErrorResponses
    {
        public static ApiErrorResponse<D> WithData<D>(string errorMessage, D data)
        {
            return new ApiErrorResponse<D>(errorMessage, data);
        }

        public static ApiErrorResponseWithMetadata<D, M> WithData<D, M>(string errorMessage, D data, M metadata)
        {
            return new ApiErrorResponseWithMetadata<D, M>(errorMessage, data, metadata);
        }

        public static ApiErrorResponse<object> WithoutData(string errorMessage)
        {
            return new ApiErrorResponse<object>(errorMessage, null);
        }
    }
}