using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Stocks_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksDashboardController : ControllerBase
    {
        private readonly IStocksDashboardService _stocksDashboardService;

        public StocksDashboardController(IStocksDashboardService stocksDashboardService)
        {
            _stocksDashboardService = stocksDashboardService;
        }

        #region get stock summary report
        [HttpGet("GetStockSummaryReport")]
        public async Task<IActionResult> GetStockSummaryReport(DateTime date)
        {
            var result = await _stocksDashboardService.GetStocksSummaryReportAsync(date);
            return Ok(result);
        }
        #endregion

        #region get intraday delivery report
        [HttpGet("GetIntradayDeliveryReport")]
        public async Task<IActionResult> GetIntradayDeliveryReport()
        {
            try
            {
                var result = await _stocksDashboardService.GetStocksIntraDeliveryReportAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region get summary chart report
        [HttpGet("GetSummaryChartReport")]
        public async Task<IActionResult> GetSummaryChartReport(DateTime fromDate, DateTime toDate)
        {
            var result = await _stocksDashboardService.GetSummaryChartReportAsync(fromDate, toDate);
            return Ok(result);
        }
        #endregion
    }
}
