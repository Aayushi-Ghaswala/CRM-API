using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IRoleMasterService
    {
        Task<int> AddRoleAsync(AddRoleMasterDto roleMasterDto);
        Task<int> AddRolePermissionAsync(AddRolePermissionDto rolePermissionDto);
        Task<int> AddUserRoleAssignmentAsync(AddUserRoleAssignmentDto userRoleAssignmentDto);
        Task<int> UpdateRoleAsync(UpdateRoleMasterDto roleMasterDto);
        Task<int> UpdateRolePermissionAsync(UpdateRolePermissionDto rolePermissionDto);
        Task<int> UpdateUserAssignRoleAsync(UpdateRoleAssignmentDto userRoleAssignmentDto);
        Task<ResponseDto<RoleMasterDto>> GetRolesAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<ResponseDto<RolePermissionDto>> GetRolePermissionsAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<ResponseDto<UserRoleAssignmentDto>> GetUserAssignRolesAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<int> DeactivateRoleAsync(int id);
        Task<int> DeactivateRolePermissionAsync(int id);
        Task<int> DeactivateRoleAssignmentAsync(int id);
    }
}
