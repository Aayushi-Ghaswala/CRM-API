using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserDashboardService
    {
        Task<List<NewUserClientCountDto>> GetNewUserClientCountAsync();
        Task<List<NewUserClientCountDto>> GetNewUserClientCountChartByDateRangeAsync(DateTime? fromDate, DateTime? toDate);
    }
}
