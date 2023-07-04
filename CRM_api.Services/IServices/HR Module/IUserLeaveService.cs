using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IUserLeaveService
    {
        Task<ResponseDto<UserLeaveDto>> GetUserLeaveAsync(string search, SortingParams sortingParams);
        Task<UserLeaveDto> GetUserLeaveByIdAsync(int id);
        Task<UserLeaveDto> GetLeaveByUserAsync(int userId);
        Task<int> AddUserLeaveAsync(AddUserLeaveDto userLeaveDto);
        Task<int> UpdateUserLeaveAsync(UpdateUserLeaveDto userLeaveDto);
        Task<int> DeactivateUserLeaveAsync(int id);
    }
}
