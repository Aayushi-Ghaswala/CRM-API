using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
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
        public async Task<ActionResult> GetLeaveTypes(int page)
        {
            try
            {
                var leaveTypes = await _leaveTypeService.GetLeaveTypesAsync(page);
                return leaveTypes.Values.Count > 0 ? Ok(leaveTypes) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add LeaveType
        [HttpPost]
        public async Task<ActionResult> AddLeaveType(AddLeaveTypeDto addLeaveTypeDto)
        {
            try
            {
                int row = await _leaveTypeService.AddLeaveTypeAsync(addLeaveTypeDto);
                return row > 0 ? Ok("Added") : BadRequest("Unable to Add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update LeaveType
        [HttpPut]
        public async Task<ActionResult> UpdateLeaveType(UpdateLeaveTypeDto updateLeaveTypeDto)
        {
            try
            {
                int row = await _leaveTypeService.UpdateLeaveTypeAsync(updateLeaveTypeDto);
                return row > 0 ? Ok("Updated") : BadRequest("Unable to update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get LeaveType By Id
        [HttpGet("GetLeaveTypeById")]
        public async Task<ActionResult<LeaveTypeDto>> GetLeaveTypeById(int leaveTypeId)
        {
            try
            {
                var leaveType = await _leaveTypeService.GetLeaveTypeById(leaveTypeId);
                return leaveType.LeaveId != 0 ? Ok(leaveType) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get LeaveType By Name
        [HttpGet("GetLeaveTypeByNamae")]
        public async Task<ActionResult<LeaveTypeDto>> GetLeaveTypeByName(string Name)
        {
            try
            {
                var leaveType = await _leaveTypeService.GetLeaveTypeByName(Name);
                return leaveType.LeaveId != 0 ? Ok(leaveType) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
