using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.HR_Module
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class UserLeaveController : ControllerBase
    {
        private readonly IUserLeaveService _userLeaveService;

        public UserLeaveController(IUserLeaveService userLeaveService)
        {
            _userLeaveService = userLeaveService;
        }

        #region Get all UserLeaves
        [HttpGet("GetUserLeave")]
        public async Task<IActionResult> GetUserLeave([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var userLeaves = await _userLeaveService.GetUserLeaveAsync(search, sortingParams);

                return Ok(userLeaves);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Leave By User
        [HttpGet("GetLeaveByUser")]
        public async Task<ActionResult<UserLeaveDto>> GetLeaveByUser(int userId)
        {
            try
            {
                var userLeave = await _userLeaveService.GetLeaveByUserAsync(userId);
                return userLeave.Id != 0 ? Ok(userLeave) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get UserLeave By Id
        [HttpGet("GetUserLeaveById")]
        public async Task<ActionResult<UserLeaveDto>> GetUserLeaveById(int id)
        {
            try
            {
                var userLeave = await _userLeaveService.GetUserLeaveByIdAsync(id);
                return userLeave.Id != 0 ? Ok(userLeave) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add UserLeave
        [HttpPost("AddUserLeave")]
        public async Task<ActionResult> AddUserLeave(AddUserLeaveDto addUserLeaveDto)
        {
            try
            {
                int row = await _userLeaveService.AddUserLeaveAsync(addUserLeaveDto);
                return row > 0 ? Ok(new { Message = "UserLeave added successfully."}) : BadRequest(new { Message = "Unable to add userLeave."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update UserLeave
        [HttpPut("UpdateUserLeave")]
        public async Task<ActionResult> UpdateUserLeave(UpdateUserLeaveDto updateUserLeaveDto)
        {
            try
            {
                int row = await _userLeaveService.UpdateUserLeaveAsync(updateUserLeaveDto);
                return row != 0 ? Ok(new { Message = "UserLeave updated successfully."}) : BadRequest(new { Message = "Unable to update userLeave."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate userLeave
        [HttpDelete("DeactivateUserLeave")]
        public async Task<IActionResult> DeactivateUserLeave(int id)
        {
            var userLeave = _userLeaveService.DeactivateUserLeaveAsync(id);
            return await userLeave !=0 ? Ok(new { Message = "UserLeave deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate userLeave."});
        }
        #endregion
    }
}
