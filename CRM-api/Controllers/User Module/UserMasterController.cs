using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMasterService _userMasterService;
        public UserMasterController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpGet]
        #region Get All Users
        public async Task<IActionResult> GetUsers(string? filterString, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            var users = await _userMasterService.GetUsersAsync(filterString, search, sortingParams);
            
            return Ok(users);
        }
        #endregion

        [HttpGet]
        #region Get User By Id
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userMasterService.GetUserMasterByIdAsync(id);
                if (user == null)
                    return BadRequest(new { Message = "User not found."});

                return Ok(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get User Count
        public async Task<IActionResult> GetUserCount()
        {
            var users = await _userMasterService.GetUserCountAsync();
            return Ok(users);
        }
        #endregion

        [HttpGet]
        #region Get All User Category
        public async Task<IActionResult> GetUserCatagories([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var categories = await _userMasterService.GetUserCategoriesAsync(search, sortingParams);
                if (categories.Values.Count == 0)
                    return BadRequest(new { Message = "Category not found."});

                return Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Users By Category Id
        public async Task<IActionResult> GetUsersByCategoryId(int categoryId, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            var users = await _userMasterService.GetUsersByCategoryIdAsync(categoryId, search, sortingParams);
            if (users.Values.Count == 0)
                return BadRequest(new { Message = "User not found"});

            return Ok(users);
        }
        #endregion

        [HttpGet]
        #region Get Category By Name
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try
            {
                var userCategory = await _userMasterService.GetCategoryByNameAsync(name);
                if (userCategory == null)
                    return BadRequest(new { Message = "User Category not found." });

                return Ok(userCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add User
        public async Task<ActionResult> AddUser(AddUserMasterDto addUser)
        {
            try
            {
               var user = await _userMasterService.AddUserAsync(addUser);
                return user != 0 ? Ok(new { Message = "User added successfully."}) : BadRequest(new { Message = "Username already exist."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update User
        public async Task<ActionResult> UpdateUser(UpdateUserMasterDto updateUser)
        {
            try
            {
                var user = await _userMasterService.UpdateUserAsync(updateUser);
                return user != 0 ? Ok(new { Message = "User updated successfully."}) : BadRequest(new { Message = "Unable to update user."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region Deactivate User
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var user = await _userMasterService.DeactivateUserAsync(id);
            return user != 0 ? Ok(new { Message = "User deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate user." });
        }
        #endregion
    }
}
