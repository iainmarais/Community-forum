namespace RestApiServer.Dto.Forum
{
    public class CreateContactRequest
    {
        public string ContactUserId { get; set; } = ""; //Refers to the user Id of the target user
        public string ContactName { get; set; } = "";
        public string ContactEmailAddress { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string AboutMessage { get; set; } = "";
        public string ContactProfileImageBase64 { get; set; } = "";
    }
}