namespace RestApiServer.Dto.Admin
{
    public class AssignRoleRequest
    {
        public required string SelectedUserId { get; set; }
        public required string SelectedRoleId { get; set; }
        public required string SelectedRoleName { get; set; }
    }
}