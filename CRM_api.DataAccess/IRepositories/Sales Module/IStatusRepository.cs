using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Sales_Module
{
    public interface IStatusRepository
    {
        Task<Response<TblStatusMaster>> GetStatues(string search, SortingParams sortingParams);
        Task<TblStatusMaster> GetStatusById(int id);
        Task<TblStatusMaster> GetStatusByName(string Name);
        Task<int> AddStatus(TblStatusMaster status);
        Task<int> UpdateStatus(TblStatusMaster status);
        Task<int> DeactivateStatus(int id);
    }
}
