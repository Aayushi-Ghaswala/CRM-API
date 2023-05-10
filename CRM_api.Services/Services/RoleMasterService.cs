using AutoMapper;
using CRM_api.DataAccess.IRepositories;
using CRM_api.Services.BuilderMethod;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.IServices;

namespace CRM_api.Services.Services
{
    public class RoleMasterService : IRoleMasterService
    {
        private readonly IRoleMasterRepository _roleMasterRepository;
        private readonly IMapper _mapper;

        public RoleMasterService(IRoleMasterRepository roleMasterRepository, IMapper mapper)
        {
            _roleMasterRepository = roleMasterRepository;
            _mapper = mapper;
        }

        #region Add Role
        public async Task<int> AddRoleAsync(AddRoleMasterDto roleMasterDto)
        {
            var role = RoleMasterBuilder.RoleMasterBuild(roleMasterDto);

            return await _roleMasterRepository.AddRole(role);
        }
        #endregion

        #region Add RolePermission
        public async Task<int> AddRolePermissionAsync(AddRolePermissionDto rolePermissionDto)
        {
            var rolePermission = RoleMasterBuilder.RolePermissionBuild(rolePermissionDto);

            return await _roleMasterRepository.AddRolePermission(rolePermission);
        }
        #endregion

        #region Add UserRoleAssignment
        public async Task<int> AddUserRoleAssignmentAsync(AddUserRoleAssignmentDto userRoleAssignmentDto)
        {
            var roleAssignment = RoleMasterBuilder.RoleAssignmentBuild(userRoleAssignmentDto);

            return await _roleMasterRepository.AddUserRoleAssignment(roleAssignment);
        }
        #endregion

        #region Update Role
        public async Task<int> UpdateRoleAsync(int id, AddRoleMasterDto roleMasterDto)
        {
            var role = await _roleMasterRepository.GetRoleById(id);
            role.RoleName = roleMasterDto.RoleName;

            return await _roleMasterRepository.UpdateRole(role);
        }
        #endregion

        #region Update Role Permission
        public async Task<int> UpdateRolePermissionAsync(int id, AddRolePermissionDto rolePermissionDto)
        {
            var rolePermission = await _roleMasterRepository.GetRolePermissionById(id);
            rolePermission.RolePermissionUpdate(rolePermissionDto.RoleId, rolePermissionDto.ModuleName, rolePermissionDto.Allow_Add
                                                , rolePermissionDto.Allow_Edit, rolePermissionDto.Allow_Delete, rolePermissionDto.Allow_View);

            return await _roleMasterRepository.UpdateRolePermission(rolePermission);
        }
        #endregion

        #region Update User Assign Role
        public async Task<int> UpdateUserAssignRoleAsync(int id, AddUserRoleAssignmentDto userRoleAssignmentDto)
        {
            var userRoleAssign = await _roleMasterRepository.GetUserAssignRoleById(id);
            userRoleAssign.UserId = userRoleAssignmentDto.UserId;
            userRoleAssign.RoleId = userRoleAssignmentDto.RoleId;

            return await _roleMasterRepository.UpdateUserRoleAssignment(userRoleAssign);
        }
        #endregion

        #region Get All Roles
        public async Task<IEnumerable<RoleMasterDto>> GetRolesAsync()
        {
            var roles = await _roleMasterRepository.GetRoles();
            var mapRoles = _mapper.Map<IEnumerable<RoleMasterDto>>(roles);

            return mapRoles;
        }
        #endregion

        #region Get All Role Permissions
        public async Task<IEnumerable<RolePermissionDto>> GetRolePermissionsAsync()
        {
            var rolePermissions = await _roleMasterRepository.GetRolePermissions();
            var mapRolePermissions = _mapper.Map<IEnumerable<RolePermissionDto>>(rolePermissions);

            return mapRolePermissions;
        }
        #endregion

        #region Get All User Assign Roles
        public async Task<IEnumerable<UserRoleAssignmentDto>> GetUserAssignRolesAsync()
        {
            var userAssignRoles = await _roleMasterRepository.GetUserAssignRoles();
            var mapUserAssignRoles = _mapper.Map<IEnumerable<UserRoleAssignmentDto>>(userAssignRoles);

            return mapUserAssignRoles;
        }
        #endregion
    }
}
