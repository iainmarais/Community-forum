namespace RestApiServer.Endpoints.ApiResponses
{
    // API client error response with optional data
    public class ApiErrorResponse<D> : ApiResponse<D>
    {
        public ApiErrorResponse(string errorMessage, D? data = default) 
            : base(false, errorMessage, data) { }
    }

    public class ApiErrorResponseWithMetadata<D, M> : ApiResponseWithMetadata<D, M>
    {
        public ApiErrorResponseWithMetadata(string errorMessage, D? data, M? metadata) 
            : base(false, errorMessage, data, metadata) { }
    }

    public static class ApiClientErrorResponses
    {
        public static ApiErrorResponse<D> WithData<D>(string errorMessage, D data) =>
            new(errorMessage, data);

        public static ApiErrorResponseWithMetadata<D, M> WithData<D, M>(string errorMessage, D data, M metadata) =>
            new(errorMessage, data, metadata);

        public static ApiErrorResponse<object> WithoutData(string errorMessage) =>
            new(errorMessage, null);
    }
}
