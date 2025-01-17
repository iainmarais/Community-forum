namespace RestApiServer.Dto.Admin
{
    public class AssignRequestToUserRequest
    {
        public required string RequestId { get; set; }
        public required string UserId { get; set; }
    }
}