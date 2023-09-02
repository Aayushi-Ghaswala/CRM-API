using CRM_api.DataAccess.Helper;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesDashboardController : ControllerBase
    {
        private readonly ISalesDashboardService _salesDashboardService;

        public SalesDashboardController(ISalesDashboardService salesDashboardService)
        {
            _salesDashboardService = salesDashboardService;
        }

        #region Get User wise Lead and Meeting
        [HttpGet("GetUserwiseLeadAndMeeting")]
        public async Task<IActionResult> GetUserwiseLeadAndMeeting(int? userId, int? campaignId)
        {
            try
            {
                var leadAndMeetings = await _salesDashboardService.GetUserwiseLeadAndMeetingAsync(userId, campaignId);
                return Ok(leadAndMeetings);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get User Wise New Client Count
        [HttpGet("GetUserWiseNewClientCount")]
        public async Task<IActionResult> GetUserWiseNewClientCount(int? userId)
        {
            try
            {
                var newClientCount = await _salesDashboardService.GetUserWiseNewClientCountAsync(userId);
                return Ok(newClientCount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get User Wise Call/Meeting Schedule Count
        [HttpGet("GetUserWiseMeetingScheduleCount")]
        public async Task<IActionResult> GetUserWiseMeetingScheduleCount(int? userId)
        {
            try
            {
                var meetingScheduleCount = await _salesDashboardService.GetUserWiseMeetingScheduleCountAsync(userId);
                return Ok(meetingScheduleCount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Meeting Wise Conversation History 
        [HttpGet("GetLeadWiseConversationHistory")]
        public async Task<IActionResult> GetLeadWiseConversationHistory(int leadId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var conversationHistories = await _salesDashboardService.GetLeadWiseConversationHistoryAsync(leadId, search, sortingParams);
                return Ok(conversationHistories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Lead Summary Chart By Date Range
        [HttpGet("GetLeadSummaryChart")]
        public async Task<IActionResult> GetLeadSummaryChart(int? assignTo, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var leadCount = await _salesDashboardService.GetLeadSummaryChartAsync(assignTo, fromDate, toDate);
                return Ok(leadCount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
