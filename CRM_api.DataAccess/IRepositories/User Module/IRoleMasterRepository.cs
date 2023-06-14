using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRoleMasterRepository
    {
        Task<int> AddRole(TblRoleMaster roleMaster);
        Task<int> AddRolePermission(TblRolePermission rolePermission);
        Task<int> AddUserRoleAssignment(TblRoleAssignment userRoleAssignment);
        Task<int> AddModule(TblModuleMaster moduleMaster);
        Task<int> UpdateRole(TblRoleMaster roleMaster);
        Task<int> UpdateRolePermission(TblRolePermission rolePermission);
        Task<int> UpdateUserRoleAssignment(TblRoleAssignment userRoleAssignment);
        Task<int> UpdateModule(TblModuleMaster moduleMaster);
        Task<int> DeactivateRole(int id);
        Task<int> DeactivateRolePermission(int id);
        Task<int> DeactivateRoleAssignment(int id);
        Task<int> DeactivateModule(int id);
        Task<TblRoleMaster> GetRoleById(int id);
        Task<TblRolePermission> GetRolePermissionById(int id);
        Task<TblRoleAssignment> GetUserAssignRoleById(int id);
        Task<Response<TblRoleMaster>> GetRoles(string search, SortingParams sortingParams);
        Task<Response<TblRolePermission>> GetRolePermissions(string search, SortingParams sortingParams);
        Task<Response<TblRoleAssignment>> GetUserAssignRoles(string search, SortingParams sortingParams);
        Task<Response<TblModuleMaster>> GetModules(string search, SortingParams sortingParams);
    }
}
