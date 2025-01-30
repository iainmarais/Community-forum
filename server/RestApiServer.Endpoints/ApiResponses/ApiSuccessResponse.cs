using RestApiServer.Endpoints.Security;

namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiSuccessResponse<D> : ApiResponse<D>
    {
        public Dictionary<string, List<AllowedUserAction>>? AllowedUserActions { get; set; }

        public ApiSuccessResponse(
            string message, 
            D? data, 
            Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null,
            int? statusCode = null)
            : base(true, message, data, ApiResponseType.Success, null, statusCode)
        {
            AllowedUserActions = allowedUserActions;
        }
    }

    public class ApiSuccessResponseWithMetadata<D, M> : ApiResponseWithMetadata<D, M>
    {
        public Dictionary<string, List<AllowedUserAction>>? AllowedUserActions { get; set; }

        public ApiSuccessResponseWithMetadata(
            string message, 
            D? data, 
            M metadata,
            Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null,
            int? statusCode = null)
            : base(true, message, data, metadata, ApiResponseType.Success, null, statusCode)
        {
            AllowedUserActions = allowedUserActions;
        }
    }

    public static class ApiSuccessResponses
    {
        public static ApiSuccessResponse<D> WithData<D>(
            string message, 
            D data, 
            Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null,
            int? statusCode = null) =>
            new(message, data, allowedUserActions, statusCode);

        public static ApiSuccessResponseWithMetadata<D, M> WithData<D, M>(
            string message, 
            D data, 
            M metadata,
            Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null,
            int? statusCode = null) =>
            new(message, data, metadata, allowedUserActions, statusCode);

        public static ApiSuccessResponse<object> WithoutData(
            string message,
            int? statusCode = null) =>
            new(message, null, null, statusCode);
    }

    // Pagination support
    public class PaginatedData<D, S>
    {
        public required D Rows { get; set; }
        public required int PageNumber { get; set; }
        public required int RowsPerPage { get; set; }
        public required int TotalPages { get; set; }
        public S? Summary { get; set; } // Optional summary
    }
}
