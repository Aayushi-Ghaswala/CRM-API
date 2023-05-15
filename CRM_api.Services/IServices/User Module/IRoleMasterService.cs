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
        Task<int> UpdateRoleAsync(UpdateRoleMasterDto roleMasterDto);
        Task<int> UpdateRolePermissionAsync(UpdateRolePermissionDto rolePermissionDto);
        Task<int> UpdateUserAssignRoleAsync(UpdateRoleAssignmentDto userRoleAssignmentDto);
        Task<int> DeactivateRoleAsync(int id);
        Task<int> DeactivateRolePermissionAsync(int id);
        Task<int> DeactivateRoleAssignmentAsync(int id);
        Task<ResponseDto<RoleMasterDto>> GetRolesAsync(int page);
        Task<ResponseDto<RolePermissionDto>> GetRolePermissionsAsync(int page);
        Task<ResponseDto<UserRoleAssignmentDto>> GetUserAssignRolesAsync(int page);
    }
}
