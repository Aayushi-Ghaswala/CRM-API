using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface ILeaveTypeService
    {
        Task<int> AddLeaveTypeAsync(AddLeaveTypeDto leaveTtypeDto);
        Task<int> UpdateLeaveTypeAsync(UpdateLeaveTypeDto leaveTypeDto);
        Task<ResponseDto<LeaveTypeDto>> GetLeaveTypesAsync(int page);
        Task<LeaveTypeDto> GetLeaveTypeById(int id);
        Task<LeaveTypeDto> GetLeaveTypeByName(string Name);
    }
}
