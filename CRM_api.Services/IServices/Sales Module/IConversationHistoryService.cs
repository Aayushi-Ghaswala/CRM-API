using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IConversationHistoryService
    {
        Task<ResponseDto<ConversationHistoryDto>> GetConversationHistoryAsync(int? meetingId, string? search, SortingParams sortingParams);
        Task<int> AddConversationHistoryAsync(AddConversationHistoryDto conversationHistoryDto);
        Task<int> UpdateConversationHistoryAsync(UpdateConversationHistoryDto conversationHistoryDto);
        Task<int> DeActivateConversationHistoryAsync(int id);
    }
}
