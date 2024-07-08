using RestApiServer.Core.Security;

namespace RestApiServer.Core.ApiResponses
{
    public class ApiSuccessResponse<T>
    {
        public string Message { get; set; }
        public T? Data  { get; set; }
        public Dictionary<string, List<AllowedUserAction>>? AllowedUserActions { get; set; }

        public ApiSuccessResponse(string message, T? data, Dictionary<string, List<AllowedUserAction>>? allowedUserActions)
        {
            Message = message;
            Data = data;
            AllowedUserActions = allowedUserActions;
        }
    }

    public class ApiSuccessResponseWithMetadata<T, M> : ApiSuccessResponse<T>
    {
        public M? Metadata { get; set; }

        public ApiSuccessResponseWithMetadata(string message, T? data, M? metadata, Dictionary<string, List<AllowedUserAction>>? allowedUserActions) : base(message, data, allowedUserActions) 
        { 
            Metadata = metadata; 
        }
    }

    public static class ApiSuccessResponses
    {
        public static ApiSuccessResponse<D> WithData<D>(string message, D data, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null)
        {
            return new ApiSuccessResponse<D>(message: message, data: data, allowedUserActions);
        }

        public static ApiSuccessResponseWithMetadata<D, M> WithData<D, M>(string message, D data, M metadata, Dictionary<string, List<AllowedUserAction>>? allowedUserActions = null)
        {
            return new ApiSuccessResponseWithMetadata<D, M>(message: message, data: data, metadata, allowedUserActions);
        }

        public static ApiSuccessResponse<object> WithoutData(string message)
        {
            return new ApiSuccessResponse<object>(message, null, null);
        }
    }

    //Pagination support
    public class PaginatedData<T, S>
    {
        public required T Rows { get; set; }
        public required int PageNum { get; set; }
        public required int RowsPerPage { get; set; }
        public required int TotalPages { get; set; }
        public required S Summary { get; set; }
    }
    //Dedicated data object class for paginated data without summary information available. 
    //Sometimes there are cases when there is no summary information to send through.
    public class PaginatedDataWithoutSummary<T>
    {
        public required T Rows { get; set; }
        public required int PageNum { get; set; }
        public required int RowsPerPage { get; set; }
        public required int TotalPages { get; set; }
    }    
}