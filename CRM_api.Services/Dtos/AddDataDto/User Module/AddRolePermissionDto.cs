namespace CRM_api.Services.Dtos.AddDataDto
{
    public class AddRolePermissionDto
    {
        public int RoleId { get; set; }
        public string ModuleName { get; set; } = null!;
        public bool AllowAdd { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowView { get; set; }
    }
}
