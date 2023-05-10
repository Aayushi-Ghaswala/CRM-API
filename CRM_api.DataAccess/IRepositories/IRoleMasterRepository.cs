using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories
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
        Task<IEnumerable<TblRoleMaster>> GetRoles();
        Task<IEnumerable<TblRolePermission>> GetRolePermissions();
        Task<IEnumerable<TblRoleAssignment>> GetUserAssignRoles();
    }
}
