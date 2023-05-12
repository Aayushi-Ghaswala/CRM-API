namespace CRM_api.Services.Dtos.ResponseDto
{
    public class UserRoleAssignmentDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
