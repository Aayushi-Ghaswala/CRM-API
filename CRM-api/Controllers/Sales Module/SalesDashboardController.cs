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
    }
}
