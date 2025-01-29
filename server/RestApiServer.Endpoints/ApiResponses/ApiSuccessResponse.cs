using RestApiServer.Endpoints.Security;

namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiSuccessResponse<D> : ApiResponse<D>
    {
        public Dictionary<string, List<AllowedUserAction>>? AllowedUserActions { get; set; }

        public ApiSuccessResponse(string message, D? data, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null)
            : base(true, message, data)
        {
            AllowedUserActions = allowedUserActions;
        }
    }

    public class ApiSuccessResponseWithMetadata<D, M> : ApiResponseWithMetadata<D, M>
    {
        public Dictionary<string, List<AllowedUserAction>>? AllowedUserActions { get; set; }

        public ApiSuccessResponseWithMetadata(string message, D? data, M? metadata, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null)
            : base(true, message, data, metadata)
        {
            AllowedUserActions = allowedUserActions;
        }
    }

    public static class ApiSuccessResponses
    {
        public static ApiSuccessResponse<D> WithData<D>(string message, D data, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null) =>
            new(message, data, allowedUserActions);

        public static ApiSuccessResponseWithMetadata<D, M> WithData<D, M>(string message, D data, M metadata, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null) =>
            new(message, data, metadata, allowedUserActions);

        public static ApiSuccessResponse<object> WithoutData(string message) =>
            new(message, null, null);
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
