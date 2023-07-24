using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ISalesDashboardService
    {
        Task<ResponseDto<ConversationHistoryDto>> GetLeadWiseConversationHistoryAsync(int leadId, SortingParams sortingParams);
    }
}
