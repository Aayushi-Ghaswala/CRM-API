using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface ILeadRepository
    {
        Task<Response<TblLeadMaster>> GetLeads(int? assignTo, string search, SortingParams sortingParams);
        Task<Response<TblInvesmentType>> GetInvestmentTypes(string search, SortingParams sortingParams);
        Task<TblLeadMaster> GetLeadById(int id);
        TblInvesmentType GetInvestmentById(int id);
        Task<TblLeadMaster> GetLeadByName(string Name);
        Task<List<TblLeadMaster>> GetLeadsForCSV(int? assignTo, string search, SortingParams sortingParams);
        int CheckMobileExist(int? id, string mobileNo);
        Task<List<TblLeadMaster>> GetUserwiseLeads(int? userId, int? campaignId, DateTime date);
        Task<(List<TblLeadMaster>, List<TblLeadMaster>)> GetLeadsByDateRange(int? assignTo, DateTime fromDate, DateTime toDate);
        Task<int> AddLead(List<TblLeadMaster> leads);
        Task<int> UpdateLead(TblLeadMaster lead);
        Task<int> DeactivateLead(int id);
    }
}
