namespace CRM_api.Services.Dtos.AddDataDto
{
    public class AddRolePermissionDto
    {
        public int RoleId { get; set; }
        public string ModuleName { get; set; } = null!;
        public Boolean Allow_Add { get; set; }
        public Boolean Allow_Edit { get; set; }
        public Boolean Allow_Delete { get; set; }
        public Boolean Allow_View { get; set; }
    }
}
