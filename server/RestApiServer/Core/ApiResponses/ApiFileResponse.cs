using Microsoft.Net.Http.Headers;

namespace RestApiServer.Core.ApiResponses
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