using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public RoleMasterDto TblRoleMaster { get; set; }
        public string? ModuleName { get; set; }
        public bool? AllowAdd { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public bool? AllowView { get; set; }
    }
}
