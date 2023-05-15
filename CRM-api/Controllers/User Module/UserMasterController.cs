using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

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
        #region Get All UserMaster Details
        public async Task<IActionResult> GetUsers(int page)
        {
            var users = await _userMasterService.GetUsersAsync(page);
            if (users.Values.Count == 0)
                return BadRequest("User Not Found...");

            return Ok(users);
        }
        #endregion

        [HttpGet]
        #region Get User Master By Id
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userMasterService.GetUserMasterById(id);
                if (user == null)
                    return BadRequest("User Not Found...");

                return Ok(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All User Categories
        public async Task<IActionResult> GetUserCatagories(int page)
        {
            try
            {
                var categories = await _userMasterService.GetUserCategoriesAsync(page);
                if (categories.Values.Count == 0)
                    return BadRequest("Category Not Found...");

                return Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All UserMaster Details
        public async Task<IActionResult> GetUsersByCategoryId(int catId, int page)
        {
            var users = await _userMasterService.GetUsersByCategoryIdAsync(page, catId);
            if (users.Values.Count == 0)
                return BadRequest("User Not Found...");

            return Ok(users);
        }
        #endregion

        [HttpPost]
        #region Add User Details
        public async Task<ActionResult> AddUser(AddUserMasterDto addUser)
        {
            try
            {
                await _userMasterService.AddUserAsync(addUser);
                return Ok("Added Successfully!!!.");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update User Master
        public async Task<ActionResult> UpdateUser(UpdateUserMasterDto updateUser)
        {
            try
            {
                var user = await _userMasterService.UpdateUserAsync(updateUser);
                return user != 0 ? Ok("User updated successfully.") : BadRequest("Unable to update user.");
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
            return user !=0 ? Ok("User deactivated successfully.") : BadRequest("Unable to deactivate user.");
        }
        #endregion
    }
}
