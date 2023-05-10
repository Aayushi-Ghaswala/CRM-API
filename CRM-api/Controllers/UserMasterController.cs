using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserMasterController : Controller
    {
        private readonly IUserMasterService _userMasterService;
        public UserMasterController(IUserMasterService userMasterService)
        {
            _userMasterService = userMasterService;
        }

        [HttpPost]
        #region AddUpdateUser Details
        public async Task<ActionResult> AddUpdateUser(AddUserMasterDto addUser, int id)
        {
            try
            {
                if (id != 0)
                {
                    await _userMasterService.UpdateUserAsync(addUser, id); 
                    return Ok("Updated Successfully!!!.");
                }
                else
                {
                    await _userMasterService.AddUserAsync(addUser);
                    return Ok("Added Successfully!!!.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All UserMaster Details
        public async Task<IActionResult> GetUsers(int page)
        {
            var users = await _userMasterService.GetUsersAsync(page);
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
        public async Task<IActionResult> GetUserCatagories()
        {
            try
            {
                var categories = await _userMasterService.GetUserCategoriesAsync();

                return Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
