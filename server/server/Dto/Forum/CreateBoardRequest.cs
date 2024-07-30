namespace RestApiServer.Dto.Forum
{
    public class CreateBoardRequest
    {
        public required string BoardName { get; set; } = "";
        public required string BoardDescription { get; set; } = "";
        public required string CategoryId   { get; set; } = "";
    }
}