using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IUserLeaveRepository
    {
        Task<Response<TblUserLeave>> GetUserLeaves(string search, SortingParams sortingParams);
        Task<TblUserLeave> GetLeavesByUser(int userId);
        Task<TblUserLeave> GetUserLeaveById(int id);
        Task<int> AddUserLeave(TblUserLeave userLeave);
        Task<int> UpdateUserLeave(TblUserLeave userLeave);
        Task<int> DeactivateUserLeave(int id);
    }
}
