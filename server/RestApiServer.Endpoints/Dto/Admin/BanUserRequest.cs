namespace RestApiServer.Endpoints.Dto.Admin
{
    public class BanUserRequest
    {
        public required string BanReason { get; set; }
        public required DateTime BanExpirationDate { get; set; }
        public string BanType { get; set; } = "";
    }
}