using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationHistoryController : ControllerBase
    {
        private readonly IConversationHistoryService _conversationHistoryService;

        public ConversationHistoryController(IConversationHistoryService conversationHistoryService)
        {
            _conversationHistoryService = conversationHistoryService;
        }

        #region Get Conversation Histories
        [HttpGet("GetConversationHistory")]
        public async Task<IActionResult> GetConversationHistory(int? meetingId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var conversationHistories = await _conversationHistoryService.GetConversationHistoryAsync(meetingId, search, sortingParams);
                return Ok(conversationHistories);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Conversation History
        [HttpPost("AddConversationHistory")]
        public async Task<IActionResult> AddConversationHistory(AddConversationHistoryDto addConversationHistory)
        {
            try
            {
                var addConversaton = await _conversationHistoryService.AddConversationHistoryAsync(addConversationHistory);
                return addConversaton != 0 ? Ok(new { Message = "Conversation history added successfully."}) : BadRequest(new {Message = "Unable to add conversation history."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Conversation History
        [HttpPut("UpdateConversationHistory")]
        public async Task<IActionResult> UpdateConversationHistory(UpdateConversationHistoryDto updateConversationHistory)
        {
            try
            {
                var updateConversaton = await _conversationHistoryService.UpdateConversationHistoryAsync(updateConversationHistory);
                return updateConversaton != 0 ? Ok(new { Message = "Conversation history updated successfully." }) : BadRequest(new { Message = "Unable to update conversation history." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DeActivate Conversation History
        [HttpDelete("DeActivateConversationHistory")]
        public async Task<IActionResult> DeActivateConversationHistory(int id)
        {
            try
            {
                var deActivateConversaton = await _conversationHistoryService.DeActivateConversationHistoryAsync(id);
                return deActivateConversaton != 0 ? Ok(new { Message = "Conversation history deleted successfully." }) : BadRequest(new { Message = "Unable to delete conversation history." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
