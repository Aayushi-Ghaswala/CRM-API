using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        #region Get all Meetings
        [HttpGet("GetMeetings")]
        public async Task<IActionResult> GetMeetings([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var meeting = await _meetingService.GetMeetingsAsync(search, sortingParams);
                return Ok(meeting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Meeting By Id
        [HttpGet("GetMeetingById")]
        public async Task<IActionResult> GetMeetingById(int meetingId)
        {
            try
            {
                var meeting = await _meetingService.GetMeetingByIdAsync(meetingId);
                return meeting.Id != 0 ? Ok(meeting) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Meeting By Purpose
        [HttpGet("GetMeetingByPurpose")]
        public async Task<IActionResult> GetMeetingByPurpose(string purpose)
        {
            try
            {
                var meeting = await _meetingService.GetMeetingByPurposeAsync(purpose);
                return meeting.Id != 0 ? Ok(meeting) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get All Meetings By Lead
        [HttpGet("GetMeetingsByLead")]
        public async Task<IActionResult> GetMeetingsByLead([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, [FromQuery] int leadId)
        {
            try
            {
                var meeting = await _meetingService.GetMeetingsByLeadIdAsync(search, sortingParams, leadId);
                return Ok(meeting);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Add Meeting
        [HttpPost("AddMeeting")]
        public async Task<IActionResult> AddMeeting([FromBody]AddMeetingDto addMeetingDto, string email)
        {
            try
            {
                int row = await _meetingService.AddMeetingAsync(addMeetingDto, email);
                return row > 0 ? Ok(new { Message = "Meeting added successfully." }) : BadRequest(new { Message = "Unable to add meeting." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Meeting
        [HttpPut("UpdateMeeting")]
        public async Task<IActionResult> UpdateMeeting(UpdateMeetingDto updateMeetingDto, string email)
        {
            try
            {
                int row = await _meetingService.UpdateMeetingAsync(updateMeetingDto, email);
                return row > 0 ? Ok(new { Message = "Meeting updated successfully." }) : BadRequest(new { Message = "Unable to update meeting." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Deactivate Meeting
        [HttpDelete("DeactivateMeeting")]
        public async Task<IActionResult> DeactivateMeeting(int id)
        {
            try
            {
                var meeting = await _meetingService.DeactivateMeetingAsync(id);
                return meeting != 0 ? Ok(new { Message = "Meeting deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate meeting." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}