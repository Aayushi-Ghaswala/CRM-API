using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface ISourceRepository
    {
        Task<Response<TblSourceMaster>> GetSources(string search, SortingParams sortingParams);
        Task<TblSourceMaster> GetSourceById(int id);
        Task<TblSourceMaster> GetSourceByName(string Name);
        Task<int> AddSource(TblSourceMaster source);
        Task<int> UpdateSource(TblSourceMaster source);
        Task<int> DeactivateSource(int id);
    }
}
