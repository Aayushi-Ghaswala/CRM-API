namespace CRM_api.Services.Dtos.AddDataDto
{
    public class AddRolePermissionDto
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowView { get; set; }
        public bool IsDeleted { get; set; }
    }
}
