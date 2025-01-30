namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiFileResponse : ApiResponse<byte[]>
    {
        public string ContentType { get; set; } = "";
        public string FileDownloadName { get; set; } = "";
        public string FileName { get; set; } = "";

        public ApiFileResponse(
            bool isSuccessful,
            string message,
            byte[] fileContents,
            string contentType,
            string fileDownloadName,
            string fileName,
            string? errorCode = null,
            int? statusCode = null)
            : base(isSuccessful, message, fileContents, ApiResponseType.File, errorCode, statusCode)
        {
            ContentType = contentType;
            FileDownloadName = fileDownloadName;
            FileName = fileName;

            if (isSuccessful)
            {
                AddInfo("ContentType", contentType);
                AddInfo("FileDownloadName", fileDownloadName);
                AddInfo("FileName", fileName);
            }
        }

        // This should be a default response for the case where no data can be sent back to the frontend.
        public ApiFileResponse()
            : base(true, "No data available", null, ApiResponseType.File)
        {
        }
    }

    public static class ApiFileResponses
    {
        public static ApiFileResponse Success(
            byte[] fileContents,
            string contentType,
            string fileDownloadName,
            string fileName,
            string message = "File retrieved successfully",
            int? statusCode = null) =>
            new(true, message, fileContents, contentType, fileDownloadName, fileName, null, statusCode);

        //Changed "null" to "[]" to make the code work, since fileContents is not nullable. 
        public static ApiFileResponse Failure(
            string errorMessage,
            string? errorCode = null,
            int? statusCode = null) =>
            new(false, errorMessage, [], "", "", "", errorCode, statusCode);
    }
}