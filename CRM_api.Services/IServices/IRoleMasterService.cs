using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.IServices
{
    public interface IRoleMasterService
    {
        Task<int> AddRoleAsync(AddRoleMasterDto roleMasterDto);
        Task<int> AddRolePermissionAsync(AddRolePermissionDto rolePermissionDto);
        Task<int> AddUserRoleAssignmentAsync(AddUserRoleAssignmentDto userRoleAssignmentDto);
        Task<int> UpdateRoleAsync(int id, AddRoleMasterDto roleMasterDto);
        Task<int> UpdateRolePermissionAsync(int id, AddRolePermissionDto rolePermissionDto);
        Task<int> UpdateUserAssignRoleAsync(int id, AddUserRoleAssignmentDto userRoleAssignmentDto);
        Task<IEnumerable<RoleMasterDto>> GetRolesAsync();
        Task<IEnumerable<RolePermissionDto>> GetRolePermissionsAsync();
        Task<IEnumerable<UserRoleAssignmentDto>> GetUserAssignRolesAsync();
    }
}
