namespace RestApiServer.Dto.Forum
{
    public class UpdateUserProfileRequest
    {
        public required string UserId { get; set; }
        public required string UserProfileImageBase64 { get; set; }
        public required string UserFirstname { get; set; }
        public required string UserLastname { get; set; }
        public required string Address { get; set; }
        public required string CityName { get; set; }
        public required string CountryName { get; set; }
        public required string PostalCode { get; set; }
        public required string Gender { get; set; }
    }
}