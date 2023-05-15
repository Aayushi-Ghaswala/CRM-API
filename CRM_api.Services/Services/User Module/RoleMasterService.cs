using AutoMapper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;

namespace CRM_api.Services.Services.User_Module
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
            var role = _mapper.Map<TblRoleMaster>(roleMasterDto);

            return await _roleMasterRepository.AddRole(role);
        }
        #endregion


        #region Add RolePermission
        public async Task<int> AddRolePermissionAsync(AddRolePermissionDto rolePermissionDto)
        {
            var rolePermission = _mapper.Map<TblRolePermission>(rolePermissionDto);
            return await _roleMasterRepository.AddRolePermission(rolePermission);
        }
        #endregion


        #region Add UserRoleAssignment
        public async Task<int> AddUserRoleAssignmentAsync(AddUserRoleAssignmentDto userRoleAssignmentDto)
        {
            var roleAssignment = _mapper.Map<TblRoleAssignment>(userRoleAssignmentDto);
            return await _roleMasterRepository.AddUserRoleAssignment(roleAssignment);
        }
        #endregion


        #region Update Role
        public async Task<int> UpdateRoleAsync(UpdateRoleMasterDto roleMasterDto)
        {
            var role = _mapper.Map<TblRoleMaster>(roleMasterDto);

            return await _roleMasterRepository.UpdateRole(role);
        }
        #endregion


        #region Update Role Permission
        public async Task<int> UpdateRolePermissionAsync(UpdateRolePermissionDto rolePermissionDto)
        {
            var updatedRolePermission = _mapper.Map<TblRolePermission>(rolePermissionDto);
            return await _roleMasterRepository.UpdateRolePermission(updatedRolePermission);
        }
        #endregion


        #region Update User Assign Role
        public async Task<int> UpdateUserAssignRoleAsync(UpdateRoleAssignmentDto userRoleAssignmentDto)
        {
            var userRoleAssign = _mapper.Map<TblRoleAssignment>(userRoleAssignmentDto);

            return await _roleMasterRepository.UpdateUserRoleAssignment(userRoleAssign);
        }
        #endregion

        #region Get All Roles
        public async Task<ResponseDto<RoleMasterDto>> GetRolesAsync(int page, string search, string sortOn)
        {
            var roles = await _roleMasterRepository.GetRoles(page, search, sortOn);
            var mapRoles = _mapper.Map<ResponseDto<RoleMasterDto>>(roles);

            return mapRoles;
        }
        #endregion

        #region Get All Role Permissions
        public async Task<ResponseDto<RolePermissionDto>> GetRolePermissionsAsync(int page, string search, string sortOn)
        {
            var rolePermissions = await _roleMasterRepository.GetRolePermissions(page, search, sortOn);
            var mapRolePermissions = _mapper.Map<ResponseDto<RolePermissionDto>>(rolePermissions);

            return mapRolePermissions;
        }
        #endregion

        #region Get All User Assign Roles
        public async Task<ResponseDto<UserRoleAssignmentDto>> GetUserAssignRolesAsync(int page, string search, string sortOn)
        {
            var userAssignRoles = await _roleMasterRepository.GetUserAssignRoles(page, search, sortOn);
            var mapUserAssignRoles = _mapper.Map<ResponseDto<UserRoleAssignmentDto>>(userAssignRoles);

            return mapUserAssignRoles;
        }
        #endregion
    }
}
