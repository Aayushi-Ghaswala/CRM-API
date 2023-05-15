using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface ILeaveTypeRepository
    {
        Task<Response<TblLeaveType>> GetLeaveTypes(int page);
        Task<int> AddLeaveType(TblLeaveType leaveType);
        Task<int> UpdateLeaveType(TblLeaveType leaveType);
        Task<TblLeaveType> GetLeaveTypeById(int id);
        Task<TblLeaveType> GetLeaveTypeByName(string Name);
    }
}
