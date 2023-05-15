using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto
{
    public class UserRoleAssignmentDto
    {
        public int Id { get; set; }
        public RoleMasterDto TblRoleMaster { get; set; }
        public UserNameDto TblUserMaster { get; set; }
        public bool IsDeleted { get; set; }
    }
}
