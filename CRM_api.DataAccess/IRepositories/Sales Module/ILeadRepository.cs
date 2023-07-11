using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface ILeadRepository
    {
        Task<Response<TblLeadMaster>> GetLeads(string search, SortingParams sortingParams);
        Task<Response<TblInvesmentType>> GetInvestmentTypes(string search, SortingParams sortingParams);
        Task<TblLeadMaster> GetLeadById(int id);
        Task<TblLeadMaster> GetLeadByName(string Name);
        Task<int> AddLead(TblLeadMaster lead);
        Task<int> UpdateLead(TblLeadMaster lead);
        Task<int> DeactivateLead(int id);
    }
}
