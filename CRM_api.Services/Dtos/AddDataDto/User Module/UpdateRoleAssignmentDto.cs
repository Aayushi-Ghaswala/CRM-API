namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateRoleAssignmentDto
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
    }
}
