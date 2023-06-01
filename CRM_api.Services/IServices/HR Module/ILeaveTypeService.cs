using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface ILeaveTypeService
    {
        Task<ResponseDto<LeaveTypeDto>> GetLeaveTypesAsync(string search, SortingParams sortingParams);
        Task<LeaveTypeDto> GetLeaveTypeByIdAsync(int id);
        Task<LeaveTypeDto> GetLeaveTypeByNameAsync(string Name);
        Task<int> AddLeaveTypeAsync(AddLeaveTypeDto leaveTtypeDto);
        Task<int> UpdateLeaveTypeAsync(UpdateLeaveTypeDto leaveTypeDto);
        Task<int> DeactivateLeaveTypeAsync(int id);
    }
}
