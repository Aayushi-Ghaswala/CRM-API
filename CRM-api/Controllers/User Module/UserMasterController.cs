﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMasterService _userMasterService;
        public UserMasterController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        #region Get All Users
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(string? filterString, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams, bool export = false)
        {
            if (export)
            {
                var users = await _userMasterService.GetUsersForCSVAsync(filterString, search, sortingParams);
                return File(users, "text/csv", "Users.csv");
            }
            else
            {
                var users = await _userMasterService.GetUsersAsync(filterString, search, sortingParams);
                return Ok(users);
            }
        }
        #endregion

        #region Get User By Id
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userMasterService.GetUserMasterByIdAsync(id);

                return user == null ? BadRequest(new { Message = "User not found." }) : Ok(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 

        #region Get User Count
        [HttpGet("GetUserCount")]
        public async Task<IActionResult> GetUserCount()
        {
            var users = await _userMasterService.GetUserCountAsync();
            return Ok(users);
        }
        #endregion 

        #region Get All Users By Category Id
        [HttpGet("GetUsersByCategoryId")]
        public async Task<IActionResult> GetUsersByCategoryId(int categoryId, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            var users = await _userMasterService.GetUsersByCategoryIdAsync(categoryId, search, sortingParams);

            return users.Count == 0 ? BadRequest(new { Message = "User not found" }) : Ok(users);
        }
        #endregion

        #region Get Family Member By UserId
        [HttpGet("GetFamilyMemberByUserId")]
        public async Task<IActionResult> GetFamilyMemberByUserId(int userId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var familyMembers = await _userMasterService.GetFamilyMemberByUserIdAsync(userId, search, sortingParams);
                return Ok(familyMembers);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Family Member By UserId
        [HttpGet("GetRelativeAccessByUserId")]
        public async Task<IActionResult> GetRelativeAccessByUserId(int userId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var relativeAccess = await _userMasterService.GetRelativeAccessByUserIdAsync(userId, search, sortingParams);
                return Ok(relativeAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Check Pan Or Aadhar Exist
        [HttpPost("PanOrAadharExist")]
        public ActionResult PanOrAadharExist(int? id, string? pan, string? aadhar)
        {
            try
            {
                if (pan is not null)
                {
                    var exist = _userMasterService.PanOrAadharExistAsync(id, pan, aadhar);
                    return exist != 0 ? Ok(exist) : BadRequest(new { Message = "Pan card already exist." });
                }
                else
                {
                    var exist = _userMasterService.PanOrAadharExistAsync(id, pan, aadhar);
                    return exist != 0 ? Ok(exist) : BadRequest(new { Message = "Aadhar card already exist." });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add User
        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser([FromForm] AddUserMasterDto addUser)
        {
            try
            {
                var user = await _userMasterService.AddUserAsync(addUser);
                return user != 0 ? Ok(new { Message = "User added successfully." }) : BadRequest(new { Message = "Unable to add user." });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update User
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromForm] UpdateUserMasterDto updateUser)
        {
            try
            {
                var user = await _userMasterService.UpdateUserAsync(updateUser);
                return user != 0 ? Ok(new { Message = "User updated successfully." }) : BadRequest(new { Message = "Unable to update user." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Relative Access
        [HttpPut("UpdateRelativeAccess")]
        public async Task<IActionResult> UpdateRelativeAccess(int id, bool isDisable)
        {
            try
            {
                var flag = await _userMasterService.UpdateRelativeAccessAsync(id, isDisable);
                return (flag != 0 && isDisable == false) ? Ok(new { Message = "Access grant successfully." }) : (flag != 0 && isDisable == true) ? Ok(new { Message = "Access disable successfully." })
                                                        : BadRequest(new { Message = "Unbale to update access" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate User
        [HttpDelete("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var user = await _userMasterService.DeactivateUserAsync(id);
            return user != 0 ? Ok(new { Message = "User deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate user." });
        }
        #endregion
    }
}
