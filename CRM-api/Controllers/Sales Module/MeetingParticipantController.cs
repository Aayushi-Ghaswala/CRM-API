using CRM_api.DataAccess.Helper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingParticipantController : ControllerBase
    {
        private readonly IMeetingParticipantService _meetingParticipantService;

        public MeetingParticipantController(IMeetingParticipantService meetingParticipantService)
        {
            _meetingParticipantService = meetingParticipantService;
        }

        #region Get all MeetingParticipants
        [HttpGet("GetMeetingParticipants")]
        public async Task<IActionResult> GetMeetingParticipants([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var meetingParticipant = await _meetingParticipantService.GetMeetingParticipantsAsync(search, sortingParams);
                return Ok(meetingParticipant);
            }
            catch (Exception)
            {
    throw;
                }
        }

#endregion


#region Get MeetingParticipant By Id
        [HttpGet("GetMeetingParticipantById")]
        public async Task<IActionResult> GetMeetingParticipantById(int meetingParticipantId)
        {
                try
            {
        var meetingParticipant = await _meetingParticipantService.GetMeetingParticipantByIdAsync(meetingParticipantId);
                        return meetingParticipant.Id != 0 ? Ok(meetingParticipant) : NoContent();
                    }
                catch (Exception)
            {
        throw;
                    }
            }
    
    #endregion
    
    
    #region Add MeetingParticipant
            [HttpPost("AddMeetingParticipant")]
            public async Task<IActionResult> AddMeetingParticipant(AddMeetingParticipantDto addMeetingParticipantDto)
        {
                    try
            {
            int row = await _meetingParticipantService.AddMeetingParticipantAsync(addMeetingParticipantDto);
                            return row > 0 ? Ok(new { Message = "MeetingParticipant added successfully." }) : BadRequest(new { Message = "Unable to add meetingParticipant." });
                        }
                    catch (Exception)
            {
            throw;
                        }
                }
        
        #endregion
        
        
        #region Update MeetingParticipant
                [HttpPut("UpdateMeetingParticipant")]
                public async Task<IActionResult> UpdateMeetingParticipant(UpdateMeetingParticipantDto updateMeetingParticipantDto)
        {
                        try
            {
                int row = await _meetingParticipantService.UpdateMeetingParticipantAsync(updateMeetingParticipantDto);
                                return row > 0 ? Ok(new { Message = "MeetingParticipant updated successfully." }) : BadRequest(new { Message = "Unable to update meetingParticipant." });
                            }
                        catch (Exception)
            {
                throw;
                            }
                    }
            
            #endregion
            
            
            #region Deactivate MeetingParticipant
                    [HttpDelete("DeactivateMeetingParticipant")]
                    public async Task<IActionResult> DeactivateMeetingParticipant(int id)
        {
                            try
            {
                    var meetingParticipant = await _meetingParticipantService.DeactivateMeetingParticipantAsync(id);
                                    return meetingParticipant != 0 ? Ok(new { Message = "MeetingParticipant deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate meetingParticipant." });
                                }
                            catch (Exception)
            {
                    throw;
                                }
                        }
                
                #endregion
                    }
            }