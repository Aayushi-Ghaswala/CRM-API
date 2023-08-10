using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ISalesDashboardService
    {
        Task<List<SalesDashboardDto>> GetUserwiseLeadAndMeetingAsync(int? userId, int? campaignId);
        Task<List<SalesDashboardDto>> GetUserWiseNewClientCountAsync(int? userId);
        Task<List<MeetingScheduleDto>> GetUserWiseMeetingScheduleCountAsync(int? userId);
        Task<ResponseDto<ConversationHistoryDto>> GetLeadWiseConversationHistoryAsync(int leadId, string? search, SortingParams sortingParams);
    }
}
