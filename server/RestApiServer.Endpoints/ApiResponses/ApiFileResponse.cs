namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiFileResponse: ApiResponse<byte[]>
    {
        public string ContentType { get; set; } = "";
        public string FileDownloadName { get; set; } = "";
        public string FileName { get; set; } = "";

        public ApiFileResponse(bool isSuccessful, string message, byte[] fileContents, string contentType, string fileDownloadName, string fileName, string? errorCode = null)
            : base(isSuccessful, message, fileContents, errorCode)
        {
            ContentType = contentType;
            FileDownloadName = fileDownloadName;
            FileName = fileName;
        }
        
        //This should be a default response for the case where no data can be sent back to the frontend.
        public ApiFileResponse()
            : base(true, string.Empty, null, null)
        {
        }
    }
}