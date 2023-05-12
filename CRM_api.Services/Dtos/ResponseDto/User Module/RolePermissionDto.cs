using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public RoleMasterDto TblRoleMaster { get; set; }
        public string ModuleName { get; set; } = null!;
        public bool Allow_Add { get; set; }
        public bool Allow_Edit { get; set; }
        public bool Allow_Delete { get; set; }
        public bool Allow_View { get; set; }
    }
}
