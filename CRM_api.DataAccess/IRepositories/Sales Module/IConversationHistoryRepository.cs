using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface IConversationHistoryRepository
    {
        Task<Response<TblConversationHistoryMaster>> GetLeadWiseConversionHistory(int leadId, string? search, SortingParams sortingParams);
        Task<int> AddConversationHistory(TblConversationHistoryMaster tblConversationHistoryMaster);
        Task<int> UpdateConversationHistory(TblConversationHistoryMaster tblConversationHistoryMaster);
        Task<int> DeActivateConversationHistory(int id);
    }
}
