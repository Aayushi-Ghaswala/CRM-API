using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingAttachmentController : ControllerBase
    {
        private readonly IMeetingAttachmentService _meetingAttachmentService;

        public MeetingAttachmentController(IMeetingAttachmentService meetingAttachmentService)
        {
            _meetingAttachmentService = meetingAttachmentService;
        }

        #region Get all MeetingAttachments
        [HttpGet("GetMeetingAttachments")]
        public async Task<IActionResult> GetMeetingAttachments([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var meetingAttachment = await _meetingAttachmentService.GetMeetingAttachmentsAsync(search, sortingParams);
                return Ok(meetingAttachment);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Get MeetingAttachment By Id
        [HttpGet("GetMeetingAttachmentById")]
        public async Task<IActionResult> GetMeetingAttachmentById(int meetingAttachmentId)
        {
            try
            {
                var meetingAttachment = await _meetingAttachmentService.GetMeetingAttachmentByIdAsync(meetingAttachmentId);
                return meetingAttachment.Id != 0 ? Ok(meetingAttachment) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Add MeetingAttachment
        [HttpPost("AddMeetingAttachment")]
        public async Task<IActionResult> AddMeetingAttachment(int meetingId, IFormFile file)
        {
            try
            {
                int row = await _meetingAttachmentService.AddMeetingAttachmentAsync(meetingId, file);
                return row > 0 ? Ok(new { Message = "MeetingAttachment added successfully." }) : BadRequest(new { Message = "Unable to add meetingAttachment." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Update MeetingAttachment
        [HttpPut("UpdateMeetingAttachment")]
        public async Task<IActionResult> UpdateMeetingAttachment(UpdateMeetingAttachmentDto updateMeetingAttachmentDto)
        {
            try
            {
                int row = await _meetingAttachmentService.UpdateMeetingAttachmentAsync(updateMeetingAttachmentDto);
                return row > 0 ? Ok(new { Message = "MeetingAttachment updated successfully." }) : BadRequest(new { Message = "Unable to update meetingAttachment." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region Deactivate MeetingAttachment
        [HttpDelete("DeactivateMeetingAttachment")]
        public async Task<IActionResult> DeactivateMeetingAttachment(string path)
        {
            try
            {
                var meetingAttachment = await _meetingAttachmentService.DeactivateMeetingAttachmentAsync(path);
                return meetingAttachment != 0 ? Ok(new { Message = "MeetingAttachment deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate meetingAttachment." });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}