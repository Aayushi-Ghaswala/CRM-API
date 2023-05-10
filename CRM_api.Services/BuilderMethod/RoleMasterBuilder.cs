using CRM_api.DataAccess.Model;
using CRM_api.Services.Dtos.AddDataDto;

namespace CRM_api.Services.BuilderMethod
{
    public class RoleMasterBuilder
    {
        public static RoleMaster RoleMasterBuild(AddRoleMasterDto addRoleMasterDto)
        {
            return new RoleMaster(addRoleMasterDto.RoleName);
        }

        public static RolePermission RolePermissionBuild(AddRolePermissionDto addRolePermissionDto)
        {
            return new RolePermission(addRolePermissionDto.RoleId, addRolePermissionDto.ModuleName, addRolePermissionDto.Allow_Add
                                        , addRolePermissionDto.Allow_Edit, addRolePermissionDto.Allow_Delete, addRolePermissionDto.Allow_View);
        }

        public static UserRoleAssignment RoleAssignmentBuild(AddUserRoleAssignmentDto roleAssign)
        {
            return new UserRoleAssignment(roleAssign.RoleId, roleAssign.UserId);
        }
    }
}
