using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStocksDashboardService
    {
        Task<List<StocksDashboardSummaryDto>> GetStocksSummaryReportAsync();
        Task<List<HoldingChartReportDto>> GetSummaryChartReportAsync(DateTime fromDate, DateTime toDate);
    }
}
