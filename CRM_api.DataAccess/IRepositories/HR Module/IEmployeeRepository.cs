using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IEmployeeRepository
    {
        Task<UserResponse> GetEmployees(int page, int catID);
        Task<int> AddEmployee(TblUserMaster userMaster);
        Task<int> UpdateEmployee(TblUserMaster userMaster);
        Task<TblUserMaster> GetEmployeebyId(int id);
    }
}
