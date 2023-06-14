namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateRolePermissionDto
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool? AllowAdd { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public bool? AllowView { get; set; }
        public bool IsDeleted { get; set; }
    }
}
