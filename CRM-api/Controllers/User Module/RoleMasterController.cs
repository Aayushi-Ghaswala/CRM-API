using CRM_api.DataAccess.Helper;
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

        [HttpGet]
        #region Get All Roles
        public async Task<IActionResult> GetRoles([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var roles = await _roleMasterService.GetRolesAsync(search, sortingParams);
                
                return Ok(roles);
            }
            catch(Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Role Permissions
        public async Task<IActionResult> GetRolePermissions([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var rolePermissions = await _roleMasterService.GetRolePermissionsAsync(search, sortingParams);
                
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
        public async Task<IActionResult> GetUserAssignRoles([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var userAssignRoles = await _roleMasterService.GetUserAssignRolesAsync(search, sortingParams);
                
                return Ok(userAssignRoles);
            }

            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Modules
        public async Task<IActionResult> GetModules([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var modules = await _roleMasterService.GetModulesAsync(search, sortingParams);

                return Ok(modules);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add Role
        public async Task<IActionResult> AddRole(AddRoleMasterDto roleMasterDto)
        {
            try
            {
                var role = await _roleMasterService.AddRoleAsync(roleMasterDto);
                return role != 0 ? Ok(new { Message = "Role added successfully."}) : BadRequest(new { Message = "Role already exists." });

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
                var flag =  await _roleMasterService.AddRolePermissionAsync(rolePermissionDto);
                return flag != 0 ? Ok(new { Message = "Rolepermission added successfully."}) : BadRequest(new { Message = "Rolepermission already exist."});

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
                var flag = await _roleMasterService.AddUserRoleAssignmentAsync(userRoleAssignmentDto);
                return flag != 0 ? Ok(new { Message = "User role assign successfully." }) : BadRequest(new { Message = "User role assign already exits." });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add Module
        public async Task<IActionResult> AddModule(AddModuleMasterDto moduleMasterDto)
        {
            try
            {
                var role = await _roleMasterService.AddModuleAsync(moduleMasterDto);
                return role != 0 ? Ok(new { Message = "Module added successfully." }) : BadRequest(new { Message = "Module already exists." });

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
                var role = await _roleMasterService.UpdateRoleAsync(roleMasterDto);
                return role !=0 ? Ok(new { Message = "Role updated successfully."}) : BadRequest(new { Message = "Unable to update role."});
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
                var rolePermission = await _roleMasterService.UpdateRolePermissionAsync(rolePermissionDto);
                return rolePermission !=0 ? Ok(new { Message = "RolePermission updated successfully."}) : BadRequest(new { Message = "Unable to update rolePermission."});
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
                var roleAssignment = await _roleMasterService.UpdateUserAssignRoleAsync(roleAssignmentDto);
                return roleAssignment !=0 ? Ok(new { Message = "RoleAssignment updated successfully."}) : BadRequest(new { Message = "Unable to update RoleAssignment."});

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update Module
        public async Task<IActionResult> UpdateModule(UpdateModuleMasterDto moduleMasterDto)
        {
            try
            {
                var role = await _roleMasterService.UpdateModuleAsync(moduleMasterDto);
                return role != 0 ? Ok(new { Message = "Module updated successfully." }) : BadRequest(new { Message = "Unable to update module." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region DeactivateRole
        public async Task<IActionResult> DeactivateRole(int id)
        {
            try
            {
                var role = await _roleMasterService.DeactivateRoleAsync(id);
                return role != 0 ? Ok(new { Message = "Role deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate role."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region Deactivate Role Permission
        public async Task<IActionResult> DeactivateRolePermission(int id)
        {
            try
            {
                var rolePermission = await _roleMasterService.DeactivateRolePermissionAsync(id);
                return rolePermission != 0 ? Ok(new { Message = "RolePermission deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate RolePermission."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region Deactivate Role Assignment
        public async Task<IActionResult> DeactivateRoleAssignment(int id)
        {
            try
            {
                var roleAssignment = await _roleMasterService.DeactivateRoleAssignmentAsync(id);
                return roleAssignment != 0 ? Ok(new { Message = "RoleAssignment deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate RoleAssignment."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region Deactivate
        public async Task<IActionResult> DeactivateModule(int id)
        {
            try
            {
                var role = await _roleMasterService.DeactivateModuleAsync(id);
                return role != 0 ? Ok(new { Message = "Module deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate module." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
