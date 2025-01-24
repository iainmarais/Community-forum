namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiResponse<D>
    {
        public string Message { get; set; }
        public D? Data  { get; set; }

        public ApiResponse(string message, D? data)
        {
            Message = message;
            Data = data;
        }
    }
    public class ApiResponseWithMetadata<D, M> : ApiResponse<D>
    {
        public M? Metadata { get; set; }

        public ApiResponseWithMetadata(string message, D? data, M? metadata) : base(message, data) 
        { 
            Metadata = metadata; 
        }
    }
}