using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IRoleMasterService
    {
        Task<int> AddRoleAsync(AddRoleMasterDto roleMasterDto);
        Task<int> AddRolePermissionAsync(AddRolePermissionDto rolePermissionDto);
        Task<int> AddUserRoleAssignmentAsync(AddUserRoleAssignmentDto userRoleAssignmentDto);
        Task<int> AddModuleAsync(AddModuleMasterDto moduleMasterDto);
        Task<int> UpdateRoleAsync(UpdateRoleMasterDto roleMasterDto);
        Task<int> UpdateRolePermissionAsync(UpdateRolePermissionDto rolePermissionDto);
        Task<int> UpdateUserAssignRoleAsync(UpdateRoleAssignmentDto userRoleAssignmentDto);
        Task<int> UpdateModuleAsync(UpdateModuleMasterDto moduleMasterDto);
        Task<ResponseDto<RoleMasterDto>> GetRolesAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<RolePermissionDto>> GetRolePermissionsAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<UserRoleAssignmentDto>> GetUserAssignRolesAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<ModuleMasterDto>> GetModulesAsync(string search, SortingParams sortingParams);
        Task<int> DeactivateRoleAsync(int id);
        Task<int> DeactivateRolePermissionAsync(int id);
        Task<int> DeactivateRoleAssignmentAsync(int id);
        Task<int> DeactivateModuleAsync(int id);
    }
}
