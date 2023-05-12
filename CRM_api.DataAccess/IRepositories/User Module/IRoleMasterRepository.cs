using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRoleMasterRepository
    {
        Task<int> AddRole(TblRoleMaster roleMaster);
        Task<int> AddRolePermission(TblRolePermission rolePermission);
        Task<int> AddUserRoleAssignment(TblRoleAssignment userRoleAssignment);
        Task<int> UpdateRole(TblRoleMaster roleMaster);
        Task<int> UpdateRolePermission(TblRolePermission rolePermission);
        Task<int> UpdateUserRoleAssignment(TblRoleAssignment userRoleAssignment);
        Task<TblRoleMaster> GetRoleById(int id);
        Task<TblRolePermission> GetRolePermissionById(int id);
        Task<TblRoleAssignment> GetUserAssignRoleById(int id);
        Task<Response<TblRoleMaster>> GetRoles(int page);
        Task<RolePermissionResponse> GetRolePermissions(int page);
        Task<UserAssignRoleResponse> GetUserAssignRoles(int page);
    }
}
