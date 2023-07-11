using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface ICampaignRepository
    {
        Task<Response<TblCampaignMaster>> GetCampaigns(string search, SortingParams sortingParams);
        Task<TblCampaignMaster> GetCampaignById(int id);
        Task<TblCampaignMaster> GetCampaignByName(string Name);
        Task<int> AddCampaign(TblCampaignMaster campaign);
        Task<int> UpdateCampaign(TblCampaignMaster campaign);
        Task<int> DeactivateCampaign(int id);
    }
}
