using CRM_api.DataAccess.Model;

namespace CRM_api.DataAccess.IRepositories
{
    public interface IRoleMasterRepository
    {
        Task<int> AddRole(RoleMaster roleMaster);
        Task<int> AddRolePermission(RolePermission rolePermission);
        Task<int> AddUserRoleAssignment(UserRoleAssignment userRoleAssignment);
        Task<int> UpdateRole(RoleMaster roleMaster);
        Task<int> UpdateRolePermission(RolePermission rolePermission);
        Task<int> UpdateUserRoleAssignment(UserRoleAssignment userRoleAssignment);
        Task<RoleMaster> GetRoleById(int id);
        Task<RolePermission> GetRolePermissionById(int id);
        Task<UserRoleAssignment> GetUserAssignRoleById(int id);
        Task<IEnumerable<RoleMaster>> GetRoles();
        Task<IEnumerable<RolePermission>> GetRolePermissions();
        Task<IEnumerable<UserRoleAssignment>> GetUserAssignRoles();
    }
}
