using AutoMapper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;

namespace CRM_api.Services.Services.Sales_Module
{
    public class ConversationHistoryService : IConversationHistoryService
    {
        private readonly IConversationHistoryRepository _conversationHistoryRepository;
        private readonly IMapper _mapper;

        public ConversationHistoryService(IMapper mapper, IConversationHistoryRepository conversationHistoryRepository)
        {
            _mapper = mapper;
            _conversationHistoryRepository = conversationHistoryRepository;
        }

        #region Add Conversation History
        public async Task<int> AddConversationHistoryAsync(AddConversationHistoryDto conversationHistoryDto)
        {
            var conversationHistory = _mapper.Map<TblConversationHistoryMaster>(conversationHistoryDto);
            return await _conversationHistoryRepository.AddConversationHistory(conversationHistory);
        }
        #endregion

        #region Update Conversation History
        public async Task<int> UpdateConversationHistoryAsync(UpdateConversationHistoryDto conversationHistoryDto)
        {
            var conversationHistory = _mapper.Map<TblConversationHistoryMaster>(conversationHistoryDto);
            return await _conversationHistoryRepository.UpdateConversationHistory(conversationHistory);
        }
        #endregion

        #region DeActivate Conversation History
        public async Task<int> DeActivateConversationHistoryAsync(int id)
        {
            return await _conversationHistoryRepository.DeActivateConversationHistory(id);
        }
        #endregion
    }
}
