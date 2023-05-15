using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleMasterController : Controller
    {
        private readonly IRoleMasterService _roleMasterService;

        public RoleMasterController(IRoleMasterService roleMasterService)
        {
            _roleMasterService = roleMasterService;
        }

        [HttpPost]
        #region Add Role
        public async Task<IActionResult> AddRole(AddRoleMasterDto roleMasterDto)
        {
            try
            {
                await _roleMasterService.AddRoleAsync(roleMasterDto);
                return Ok("Role Added Successfully!!!.");

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update Role
        public async Task<IActionResult> UpdateRole(UpdateRoleMasterDto roleMasterDto)
        {
            try
            {
                await _roleMasterService.UpdateRoleAsync(roleMasterDto);
                return Ok("Role Updated Successfully...");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add RolePermission
        public async Task<IActionResult> AddRolePermission([FromForm] AddRolePermissionDto rolePermissionDto)
        {
            try
            {
                await _roleMasterService.AddRolePermissionAsync(rolePermissionDto);
                return Ok("RolePermission Added Successfully!!!.");

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update Role Permission
        public async Task<IActionResult> UpdateRolePermission(UpdateRolePermissionDto rolePermissionDto)
        {
            try
            {
                await _roleMasterService.UpdateRolePermissionAsync(rolePermissionDto);
                return Ok("Role Updated Successfully...");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        [HttpPost]
        #region Add User Role Assignment
        public async Task<IActionResult> AddUserRoleAssignment(AddUserRoleAssignmentDto userRoleAssignmentDto)
        {
            try
            {
                await _roleMasterService.AddUserRoleAssignmentAsync(userRoleAssignmentDto);
                return Ok("User Role Assign Successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update User Assign Role
        public async Task<IActionResult> UpdateRoleAssign(UpdateRoleAssignmentDto roleAssignmentDto)
        {
            try
            {
                await _roleMasterService.UpdateUserAssignRoleAsync(roleAssignmentDto);
                return Ok("Role Updated Successfully...");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Roles
        public async Task<IActionResult> GetRoles(int page, string? search, string? sortOn)
        {
            try
            {
                var roles = await _roleMasterService.GetRolesAsync(page, search, sortOn);
                if (roles.Values.Count == 0)
                    return BadRequest("Role Not Found...");

                return Ok(roles);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Role Permissions
        public async Task<IActionResult> GetRolePermissions(int page, string? search, string sortOn)
        {
            try
            {
                var rolePermissions = await _roleMasterService.GetRolePermissionsAsync(page, search, sortOn);
                if (rolePermissions.Values.Count == 0)
                    return BadRequest("Role Permission Not Found");

                return Ok(rolePermissions);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All User Assign Role
        public async Task<IActionResult> GetUserAssignRoles(int page, string? search, string? sortOn)
        {
            try
            {
                var userAssignRoles = await _roleMasterService.GetUserAssignRolesAsync(page, search, sortOn);
                if (userAssignRoles.Values.Count == 0)
                    return BadRequest("User Assign Role Not Found...");

                return Ok(userAssignRoles);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion  
    }
}
