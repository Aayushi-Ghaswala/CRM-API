using CRM_api.Services.Dtos.AddDataDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IConversationHistoryService
    {
        Task<int> AddConversationHistoryAsync(AddConversationHistoryDto conversationHistoryDto);
        Task<int> UpdateConversationHistoryAsync(UpdateConversationHistoryDto conversationHistoryDto);
        Task<int> DeActivateConversationHistoryAsync(int id);
    }
}
