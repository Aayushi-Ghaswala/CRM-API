using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;
using Microsoft.EntityFrameworkCore;

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

        #region Deactivate Role
        public async Task<int> DeactivateRoleAsync(int id)
        {
            return await _roleMasterRepository.DeactivateRole(id);
        }
        #endregion

        #region Deactivate Role Permission
        public async Task<int> DeactivateRolePermissionAsync(int id)
        {
            return await _roleMasterRepository.DeactivateRolePermission(id);
        }
        #endregion

        #region Deactivate Role Assignment
        public async Task<int> DeactivateRoleAssignmentAsync(int id)
        {
            return await _roleMasterRepository.DeactivateRoleAssignment(id);
        }
        #endregion

        #region Get All Roles
        public async Task<ResponseDto<RoleMasterDto>> GetRolesAsync(string search, SortingParams sortingParams)
        {
            var roles = await _roleMasterRepository.GetRoles(search, sortingParams);
            var mapRoles = _mapper.Map<ResponseDto<RoleMasterDto>>(roles);

            return mapRoles;
        }
        #endregion

        #region Get All Role Permissions
        public async Task<ResponseDto<RolePermissionDto>> GetRolePermissionsAsync(string search, SortingParams sortingParams)
        {
            var rolePermissions = await _roleMasterRepository.GetRolePermissions(search, sortingParams);
            var mapRolePermissions = _mapper.Map<ResponseDto<RolePermissionDto>>(rolePermissions);

            return mapRolePermissions;
        }
        #endregion

        #region Get All User Assign Roles
        public async Task<ResponseDto<UserRoleAssignmentDto>> GetUserAssignRolesAsync(string search, SortingParams sortingParams)
        {
            var userAssignRoles = await _roleMasterRepository.GetUserAssignRoles(search, sortingParams);
            var mapUserAssignRoles = _mapper.Map<ResponseDto<UserRoleAssignmentDto>>(userAssignRoles);

            return mapUserAssignRoles;
        }
        #endregion
    }
}
