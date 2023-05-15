using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

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
        Task<int> DeactivateRole(int id);
        Task<int> DeactivateRolePermission(int id);
        Task<int> DeactivateRoleAssignment(int id);
        Task<TblRoleMaster> GetRoleById(int id);
        Task<TblRolePermission> GetRolePermissionById(int id);
        Task<TblRoleAssignment> GetUserAssignRoleById(int id);
        Task<Response<TblRoleMaster>> GetRoles(int page);
        Task<Response<TblRolePermission>> GetRolePermissions(int page);
        Task<Response<TblRoleAssignment>> GetUserAssignRoles(int page);
    }
}
