namespace RestApiServer.Dto.Admin
{
    public class EditUserRequest
    {
        public required string UserId { get; set; }
        public string UserFirstname { get; set; } = "";
        public string UserLastname { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Username { get; set; } = "";
        //Add more props as needed
    }
}