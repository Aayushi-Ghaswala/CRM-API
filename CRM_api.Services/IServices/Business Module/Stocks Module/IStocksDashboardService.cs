using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStocksDashboardService
    {
        Task<List<StocksDashboardSummaryDto>> GetStocksSummaryReportAsync(DateTime date);
        Task<List<StocksDashboardIntraDeliveryDto>> GetStocksIntraDeliveryReportAsync(DateTime date);
        Task<List<HoldingChartReportDto>> GetSummaryChartReportAsync(DateTime fromDate, DateTime toDate);
    }
}
