using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.HR_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ILeaveTypeService _leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService;
        }

        #region Get all LeaveTypes
        [HttpGet]
        public async Task<IActionResult> GetLeaveTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var leaveTypes = await _leaveTypeService.GetLeaveTypesAsync(search, sortingParams);
                return Ok(leaveTypes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get LeaveType By Id
        [HttpGet("GetLeaveTypeById")]
        public async Task<IActionResult> GetLeaveTypeById(int leaveTypeId)
        {
            try
            {
                var leaveType = await _leaveTypeService.GetLeaveTypeByIdAsync(leaveTypeId);
                return leaveType.LeaveId != 0 ? Ok(leaveType) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get LeaveType By Name
        [HttpGet("GetLeaveTypeByNamae")]
        public async Task<IActionResult> GetLeaveTypeByName(string Name)
        {
            try
            {
                var leaveType = await _leaveTypeService.GetLeaveTypeByNameAsync(Name);
                return leaveType.LeaveId != 0 ? Ok(leaveType) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add LeaveType
        [HttpPost]
        public async Task<IActionResult> AddLeaveType(AddLeaveTypeDto addLeaveTypeDto)
        {
            try
            {
                int row = await _leaveTypeService.AddLeaveTypeAsync(addLeaveTypeDto);
                return row > 0 ? Ok(new { Message = "LeaveType added successfully."}) : BadRequest(new { Message = "Unable to add leaveType."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update LeaveType
        [HttpPut]
        public async Task<IActionResult> UpdateLeaveType(UpdateLeaveTypeDto updateLeaveTypeDto)
        {
            try
            {
                int row = await _leaveTypeService.UpdateLeaveTypeAsync(updateLeaveTypeDto);
                return row > 0 ? Ok(new { Message = "LeaveType updated successfully."}) : BadRequest(new { Message = "Unable to update leaveType."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate LeaveType
        [HttpDelete]
        public async Task<IActionResult> DeactivateLeaveType(int id)
        {
            try
            {
                var leaveType = await _leaveTypeService.DeactivateLeaveTypeAsync(id);
                return leaveType != 0 ? Ok(new { Message = "LeaveType deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate leaveType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
