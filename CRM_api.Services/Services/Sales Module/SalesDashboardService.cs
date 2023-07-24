using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using static CRM_api.Services.Helper.ConstantValue.InvesmentTypeConstant;

namespace CRM_api.Services.Services.Sales_Module
{
    public class SalesDashboardService : ISalesDashboardService
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IConversationHistoryRepository _conversationHistoryRepository;
        private readonly IMapper _mapper;

        public SalesDashboardService(ISalesRepository salesRepository, IUserMasterRepository userMasterRepository, IMapper mapper, ILeadRepository leadRepository, IConversationHistoryRepository conversationHistoryRepository)
        {
            _salesRepository = salesRepository;
            _userMasterRepository = userMasterRepository;
            _mapper = mapper;
            _leadRepository = leadRepository;
            _conversationHistoryRepository = conversationHistoryRepository;
        }

        #region Get Meeting Wise Conversation History 
        public async Task<ResponseDto<ConversationHistoryDto>> GetLeadWiseConversationHistoryAsync(int leadId, SortingParams sortingParams)
        {
            var conversationHistories = await _conversationHistoryRepository.GetLeadWiseConversionHistory(leadId, sortingParams);
            var mapConversationHistories = _mapper.Map<ResponseDto<ConversationHistoryDto>>(conversationHistories);

            return mapConversationHistories;
        }
        #endregion
    }
}
