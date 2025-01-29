namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiResponse<D>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public D? Data  { get; set; }
        public string? ErrorCode { get; set; }


        public ApiResponse(bool isSuccessful, string message, D? data, string? errorCode = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            ErrorCode = errorCode;
            Data = data;
        }
    }
    public class ApiResponseWithMetadata<D, M> : ApiResponse<D>
    {
        public M? Metadata { get; set; }

        public ApiResponseWithMetadata(bool isSuccessful, string message, D? data, M? metadata, string? errorCode = null) 
            : base(isSuccessful, message, data, errorCode)
        { 
            Metadata = metadata;
        }
    }
}