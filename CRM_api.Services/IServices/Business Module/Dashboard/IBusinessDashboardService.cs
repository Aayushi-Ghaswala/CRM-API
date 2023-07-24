using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Dashboard
{
    public interface IBusinessDashboardService
    {
        Task<List<ClientReportDto<ClientCurrentInvSnapshotDto>>> GetClientCurrentInvSnapshotAsync(int? userId, bool? isZero, string search);
        Task<List<ClientReportDto<ClientMonthlyTransSnapshotDto>>> GetClientMonthlyTransSnapshotAsync(int? userId, int? month, int? year, bool? isZero, string search);
        Task<MFMonthlyChartDto> GetMonthlyChartAsync();
        int SendCurrentInvSnapshotEmailAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto);
        int SendCurrentInvSnapshotSMSAsync(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto);
        int SendMonthlyTransSnapshotEmailAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto);
        int SendMonthlyTransSnapshotSMSAsync(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto);
    }
}
