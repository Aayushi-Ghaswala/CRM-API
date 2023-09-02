using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutalfundDashBoardService
    {
        Task<List<TopTenSchemeDto>> GetTopTenSchemeByInvestmentAsync();
        Task<List<HoldingChartReportDto>> GetMFHoldingSummaryAsync(DateTime? fromDate, DateTime? toDate);
        Task<List<TimeWiseMutualFundSummaryDto>> GetMFSummaryTimeWiseAsync();
    }
}
