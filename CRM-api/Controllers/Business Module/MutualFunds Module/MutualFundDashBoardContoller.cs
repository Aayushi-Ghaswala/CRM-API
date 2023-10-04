using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.MutualFunds_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutualFundDashBoardContoller : ControllerBase
    {
        private readonly IMutalfundDashBoardService _mutalfundDashBoardService;

        public MutualFundDashBoardContoller(IMutalfundDashBoardService mutalfundDashBoardService)
        {
            _mutalfundDashBoardService = mutalfundDashBoardService;
        }

        #region Get Top 10 Scheme By Investment Amount
        [HttpGet("GetTopTenSchemeByInvestment")]
        public async Task<IActionResult> GetTopTenSchemeByInvestment()
        {
            var schemes = await _mutalfundDashBoardService.GetTopTenSchemeByInvestmentAsync();
            return Ok(schemes);
        }
        #endregion

        #region Get MF User Count In DateRange
        [HttpGet("GetMFHoldingSummary")]
        public async Task<IActionResult> GetMFHoldingSummary(DateTime? fromDate, DateTime? toDate)
        {
            var mfUserCount = await _mutalfundDashBoardService.GetMFHoldingSummaryAsync(fromDate, toDate);
            return Ok(mfUserCount);
        }
        #endregion

        #region Get All Mutual Fund Summary For Different Time
        [HttpGet("GetMFSummaryTimeWise")]
        public async Task<IActionResult> GetMFSummaryTimeWise(DateTime date)
        {
            var mfTimeWiseData = await _mutalfundDashBoardService.GetMFSummaryTimeWiseAsync(date);
            return Ok(mfTimeWiseData);
        }
        #endregion
    }
}
