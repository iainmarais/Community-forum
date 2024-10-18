namespace RestApiServer.Dto.Admin
{
    public class AssignRoleRequest
    {
        public required string SelectedUserId { get; set; }
        public required string SelectedRoleType { get; set; }
    }
}