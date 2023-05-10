using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers
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
        public async Task<IActionResult> AddUpdateRole(AddRoleMasterDto roleMasterDto, int id)
        {
            try
            {
                if(id != 0)
                {
                    await _roleMasterService.UpdateRoleAsync(id, roleMasterDto);
                    return Ok("Role Updated Successfully...");
                }
                else
                {
                    await _roleMasterService.AddRoleAsync(roleMasterDto);
                    return Ok("Role Added Successfully!!!.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add RolePermission
        public async Task<IActionResult> AddUpdateRolePermission([FromForm] AddRolePermissionDto rolePermissionDto, int id)
        {
            try
            {
                if(id != 0)
                {
                    await _roleMasterService.UpdateRolePermissionAsync(id, rolePermissionDto);
                    return Ok("Role Permission Updated Successfully...");
                }
                else
                {
                    await _roleMasterService.AddRolePermissionAsync(rolePermissionDto);
                    return Ok("RolePermission Added Successfully!!!.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add User Role Assignment
        public async Task<IActionResult> AddUpdateUserRoleAssignment(AddUserRoleAssignmentDto userRoleAssignmentDto, int id)
        {
            try
            {
                if(id != 0)
                {
                    await _roleMasterService.UpdateUserAssignRoleAsync(id, userRoleAssignmentDto);
                    return Ok("User Role Assign Updated Successfully...");
                }
                else
                {
                    await _roleMasterService.AddUserRoleAssignmentAsync(userRoleAssignmentDto);
                    return Ok("User Role Assign Successfully");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Roles
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleMasterService.GetRolesAsync();

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
        public async Task<IActionResult> GetRolePermissions()
        {
            try
            {
                var rolePermissions = await _roleMasterService.GetRolePermissionsAsync();

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
        public async Task<IActionResult> GetUserAssignRoles()
        {
            try
            {
                var userAssignRoles = await _roleMasterService.GetUserAssignRolesAsync();

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
