using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Dashboard
{
    public interface IBusinessDashboardService
    {
        Task<ResponseDto<ClientReportDto<ClientCurrentInvSnapshotDto>>> GetClientCurrentInvSnapshotAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<ClientReportDto<ClientMonthlyTransSnapshotDto>>> GetClientMonthlyTransSnapshotAsync(string search, SortingParams sortingParams);
        Task<MFMonthlyChartDto> GetMonthlyChartAsync();
        int SendCurrentInvSnapshotEmailAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto);
        int SendCurrentInvSnapshotSMSAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto);
        int SendMonthlyTransSnapshotEmailAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto);
        int SendMonthlyTransSnapshotSMSAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto);
    }
}
