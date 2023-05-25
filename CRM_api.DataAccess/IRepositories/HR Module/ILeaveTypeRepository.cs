using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface ILeaveTypeRepository
    {
        Task<Response<TblLeaveType>> GetLeaveTypes(string search, SortingParams sortingParams);
        Task<TblLeaveType> GetLeaveTypeById(int id);
        Task<TblLeaveType> GetLeaveTypeByName(string Name);
        Task<int> AddLeaveType(TblLeaveType leaveType);
        Task<int> UpdateLeaveType(TblLeaveType leaveType);
        Task<int> DeactivateLeaveType(int id);
    }
}
