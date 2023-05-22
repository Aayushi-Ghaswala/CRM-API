using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
                return Ok(new { Message = "Role added successfully."});

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

        [HttpPost]
        #region Add RolePermission
        public async Task<IActionResult> AddRolePermission([FromForm] AddRolePermissionDto rolePermissionDto)
        {
            try
            {
                await _roleMasterService.AddRolePermissionAsync(rolePermissionDto);
                return Ok(new { Message = "RolePermission added successfully."});

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

        [HttpPost]
        #region Add User Role Assignment
        public async Task<IActionResult> AddUserRoleAssignment(AddUserRoleAssignmentDto userRoleAssignmentDto)
        {
            try
            {
                await _roleMasterService.AddUserRoleAssignmentAsync(userRoleAssignmentDto);
                return Ok(new { Message = "User Role Assign successfully." });

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

        [HttpGet]
        #region Get All Roles
        public async Task<IActionResult> GetRoles([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var roles = await _roleMasterService.GetRolesAsync(data, sortingParams);
                
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
        public async Task<IActionResult> GetRolePermissions([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var rolePermissions = await _roleMasterService.GetRolePermissionsAsync(data, sortingParams);
                
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
        public async Task<IActionResult> GetUserAssignRoles([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var userAssignRoles = await _roleMasterService.GetUserAssignRolesAsync(data, sortingParams);
                
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
