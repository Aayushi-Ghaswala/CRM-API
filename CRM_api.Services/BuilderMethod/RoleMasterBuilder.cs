using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;

namespace CRM_api.Services.BuilderMethod
{
    public class RoleMasterBuilder
    {
        public static TblRoleMaster RoleMasterBuild(AddRoleMasterDto addRoleMasterDto)
        {
            return new TblRoleMaster(addRoleMasterDto.RoleName);
        }

        public static TblRolePermission RolePermissionBuild(AddRolePermissionDto addRolePermissionDto)
        {
            return new TblRolePermission(addRolePermissionDto.RoleId, addRolePermissionDto.ModuleName, addRolePermissionDto.Allow_Add
                                        , addRolePermissionDto.Allow_Edit, addRolePermissionDto.Allow_Delete, addRolePermissionDto.Allow_View);
        }

        public static TblRoleAssignment RoleAssignmentBuild(AddUserRoleAssignmentDto roleAssign)
        {
            return new TblRoleAssignment(roleAssign.RoleId, roleAssign.UserId);
        }
    }
}
