namespace RestApiServer.Endpoints.ApiResponses
{
    public class ApiFileResponse
    {
        public string? ResponseMessage { get; set; }
        public byte[] FileContents { get; set; } = [];
        public string ContentType { get; set; } = "";
        public string FileDownloadName { get; set; } = "";
        public string FileName { get; set; } = "";
    }
}