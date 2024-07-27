namespace RestApiServer.Dto.Forum
{
    public class UpdateUserProfileRequest
    {
        public required string UserId { get; set; }
        public required string UserProfileImageBase64 { get; set; }
        public required string UserFirstname { get; set; }
        public required string UserLastname { get; set; }
    }
}