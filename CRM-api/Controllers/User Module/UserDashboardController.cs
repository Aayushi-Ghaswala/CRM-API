using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly IUserDashboardService _userDashboardService;

        public UserDashboardController(IUserDashboardService userDashboardService)
        {
            _userDashboardService = userDashboardService;
        }

        #region Get New User And Client Count
        [HttpGet("GetNewUserClientCount")]
        public async Task<IActionResult> GetNewUserClientCount()
        {
            try
            {
                var newUserClientCount = await _userDashboardService.GetNewUserClientCountAsync();
                return Ok(newUserClientCount);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get New User And Client Count For Chart By Date Range
        [HttpGet("GetNewUserClientCountChartByDateRange")]
        public async Task<IActionResult> GetNewUserClientCountChartByDateRange(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var newUserClientCountChart = await _userDashboardService.GetNewUserClientCountChartByDateRangeAsync(fromDate, toDate);
                return Ok(newUserClientCountChart);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
