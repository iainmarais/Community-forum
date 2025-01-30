namespace RestApiServer.Endpoints.ApiResponses
{
    public enum ApiResponseType
    {
        Success,
        Failure,
        Error,
        File
    }

    public class ApiResponse<D>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public D? Data { get; set; }
        public string? ErrorCode { get; set; }
        public ApiResponseType ResponseType { get; set; }
        public int? StatusCode { get; set; }
        public Dictionary<string, string>? AdditionalInfo { get; set; }

        public ApiResponse(
            bool isSuccessful, 
            string message, 
            D? data, 
            ApiResponseType responseType,
            string? errorCode = null,
            int? statusCode = null,
            Dictionary<string, string>? additionalInfo = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Data = data;
            ResponseType = responseType;
            ErrorCode = errorCode;
            StatusCode = statusCode;
            AdditionalInfo = additionalInfo;
        }

        protected ApiResponse<D> AddInfo(string key, string value)
        {
            AdditionalInfo ??= new Dictionary<string, string>();
            AdditionalInfo[key] = value;
            return this;
        }
    }

    public class ApiResponseWithMetadata<D, M> : ApiResponse<D>
    {
        public M? Metadata { get; set; }

        public ApiResponseWithMetadata(
            bool isSuccessful, 
            string message, 
            D? data, 
            M? metadata,
            ApiResponseType responseType,
            string? errorCode = null,
            int? statusCode = null,
            Dictionary<string, string>? additionalInfo = null) 
            : base(isSuccessful, message, data, responseType, errorCode, statusCode, additionalInfo)
        {
            Metadata = metadata;
        }
    }
}