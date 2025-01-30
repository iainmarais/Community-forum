namespace RestApiServer.Endpoints.ApiResponses
{
    // API client error response with optional data
    public class ApiErrorResponse<D> : ApiResponse<D>
    {
        public ApiErrorResponse(
            string errorMessage, 
            D? data = default,
            string? errorCode = null,
            int? statusCode = null) 
            : base(false, errorMessage, data, ApiResponseType.Error, errorCode, statusCode) { }
    }

    public class ApiFailureResponse<D> : ApiResponse<D>
    {
        public ApiFailureResponse(
            string failureMessage,
            D? data = default,
            string? errorCode = null,
            int? statusCode = null)
            : base(false, failureMessage, data, ApiResponseType.Failure, errorCode, statusCode) { }
    }

    public class ApiErrorResponseWithMetadata<D, M> : ApiResponseWithMetadata<D, M>
    {
        public ApiErrorResponseWithMetadata(
            string errorMessage,
            D? data,
            M metadata,
            string? errorCode = null,
            int? statusCode = null)
            : base(false, errorMessage, data, metadata, ApiResponseType.Error, errorCode, statusCode) { }
    }

    public class ApiFailureResponseWithMetadata<D, M> : ApiResponseWithMetadata<D, M>
    {
        public ApiFailureResponseWithMetadata(
            string failureMessage,
            D? data,
            M metadata,
            string? errorCode = null,
            int? statusCode = null)
            : base(false, failureMessage, data, metadata, ApiResponseType.Failure, errorCode, statusCode) { }
    }

    public static class ApiClientErrorResponses
    {
        public static ApiErrorResponse<D> WithData<D>(
            string errorMessage,
            D? data,
            string? errorCode = null,
            int? statusCode = null) =>
            new(errorMessage, data, errorCode, statusCode);

        public static ApiErrorResponseWithMetadata<D, M> WithData<D, M>(
            string errorMessage,
            D? data,
            M metadata,
            string? errorCode = null,
            int? statusCode = null) =>
            new(errorMessage, data, metadata, errorCode, statusCode);

        public static ApiErrorResponse<object> WithoutData(
            string errorMessage,
            string? errorCode = null,
            int? statusCode = null) =>
            new(errorMessage, null, errorCode, statusCode);
    }

    public static class ApiFailureResponses
    {
        public static ApiFailureResponse<D> WithData<D>(
            string failureMessage,
            D? data,
            string? errorCode = null,
            int? statusCode = null) =>
            new(failureMessage, data, errorCode, statusCode);

        public static ApiFailureResponseWithMetadata<D, M> WithData<D, M>(
            string failureMessage,
            D? data,
            M metadata,
            string? errorCode = null,
            int? statusCode = null) =>
            new(failureMessage, data, metadata, errorCode, statusCode);

        public static ApiFailureResponse<object> WithoutData(
            string failureMessage,
            string? errorCode = null,
            int? statusCode = null) =>
            new(failureMessage, null, errorCode, statusCode);
    }
}
