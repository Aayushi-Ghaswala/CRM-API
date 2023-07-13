using CRM_api.DataAccess.Helper;
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
                if (user == null)
                    return BadRequest(new { Message = "User not found." });

                return Ok(user);
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

        #region Get All User Category
        [HttpGet("GetUserCatagories")]
        public async Task<IActionResult> GetUserCatagories([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var categories = await _userMasterService.GetUserCategoriesAsync(search, sortingParams);
                if (categories.Values.Count == 0)
                    return BadRequest(new { Message = "Category not found." });

                return Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Users By Category Id
        [HttpGet("GetUsersByCategoryId")]
        public async Task<IActionResult> GetUsersByCategoryId(int categoryId, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            var users = await _userMasterService.GetUsersByCategoryIdAsync(categoryId, search, sortingParams);
            if (users.Values.Count == 0)
                return BadRequest(new { Message = "User not found" });

            return Ok(users);
        }
        #endregion

        #region Get Category By Name
        [HttpGet("GetCategoryByName")]
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
        public async Task<ActionResult> AddUser(AddUserMasterDto addUser)
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
        public async Task<ActionResult> UpdateUser(UpdateUserMasterDto updateUser)
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
