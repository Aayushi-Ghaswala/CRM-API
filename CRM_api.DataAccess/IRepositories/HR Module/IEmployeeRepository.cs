using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IEmployeeRepository
    {
        Task<Response<TblUserMaster>> GetEmployees(int categoryId, Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<TblUserMaster> GetEmployeebyId(int id);
        Task<int> AddEmployee(TblUserMaster userMaster);
        Task<int> UpdateEmployee(TblUserMaster userMaster);
        Task<int> DeactivateEmployee(int id);
    }
}
