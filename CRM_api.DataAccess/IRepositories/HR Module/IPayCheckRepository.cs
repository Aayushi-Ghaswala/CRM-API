using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IPayCheckRepository
    {
        Task<Response<TblPayCheck>> GetPayChecks(string search, SortingParams sortingParams);
        Task<TblPayCheck> GetPayChecksByDesignation(int designationId);
        Task<TblPayCheck> GetPayCheckById(int id);
        Task<int> AddPayCheck(TblPayCheck payCheck);
        Task<int> UpdatePayCheck(TblPayCheck payCheck);
        Task<int> DeactivatePayCheck(int id);
    }
}
