using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCategoryController : ControllerBase
    {
        private readonly IUserCategoryService _userCategoryService;

        public UserCategoryController(IUserCategoryService userCategoryService)
        {
            _userCategoryService = userCategoryService;
        }

        #region Get All User Category
        [HttpGet("GetUserCatagories")]
        public async Task<IActionResult> GetUserCatagories([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var categories = await _userCategoryService.GetUserCategoriesAsync(search, sortingParams);

                return categories.Values.Count == 0 ? BadRequest(new { Message = "Category not found." }) : Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Category By Name
        [HttpGet("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            try
            {
                var userCategory = await _userCategoryService.GetCategoryByNameAsync(name);

                return userCategory == null ? BadRequest(new { Message = "User Category not found." }) : Ok(userCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add User Category
        [HttpPost("AddUserCategory")]
        public async Task<IActionResult> AddUserCategory(AddUserCategoryDto addUserCategory)
        {
            try
            {
                var flag = await _userCategoryService.AddUserCategoryAsync(addUserCategory);
                return flag != 0 ? Ok(new { Message = "User category added successfully." }) : BadRequest(new { Message = "Unable to add user category." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update User Category
        [HttpPut("UpdateUserCategory")]
        public async Task<IActionResult> UpdateUserCategory(UpdateUserCategoryDto updateUserCategory)
        {
            try
            {
                var flag = await _userCategoryService.UpdateUserCategoryAsync(updateUserCategory);
                return flag != 0 ? Ok(new { Message = "User category updated successfully." }) : BadRequest(new { Message = "Unable to update user category." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region De-Activate User Category
        [HttpDelete("DeActivateUserCategory")]
        public async Task<IActionResult> DeActivateUserCategory(int id)
        {
            try
            {
                var flag = await _userCategoryService.DeActivateUserCategoryAsync(id);
                return flag != 0 ? Ok(new { Message = "User category de-activated successfully." }) : BadRequest(new { Message = "Unable to de-activate user category." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
