using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<IActionResult> GetLeaveTypes([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
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
                var leaveTypes = await _leaveTypeService.GetLeaveTypesAsync(data, sortingParams);
                return Ok(leaveTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get LeaveType By Id
        [HttpGet("GetLeaveTypeById")]
        public async Task<IActionResult> GetLeaveTypeById(int leaveTypeId)
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
        public async Task<IActionResult> GetLeaveTypeByName(string Name)
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

        #region Add LeaveType
        [HttpPost]
        public async Task<IActionResult> AddLeaveType(AddLeaveTypeDto addLeaveTypeDto)
        {
            try
            {
                int row = await _leaveTypeService.AddLeaveTypeAsync(addLeaveTypeDto);
                return row > 0 ? Ok(new { Message = "LeaveType added successfully."}) : BadRequest(new { Message = "Unable to add leaveType."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate LeaveType
        [HttpDelete]
        public async Task<IActionResult> DeactivateLeaveType(int id)
        {
            var leaveType = await _leaveTypeService.DeactivateLeaveTypeAsync(id);
            return leaveType != 0 ? Ok(new { Message = "LeaveType deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate leaveType."});
        }
        #endregion
    }
}
