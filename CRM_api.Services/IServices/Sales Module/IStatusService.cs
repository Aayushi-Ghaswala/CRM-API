using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface IStatusService
    {
        Task<ResponseDto<StatusDto>> GetStatuesAsync(string search, SortingParams sortingParams);
        Task<StatusDto> GetStatusByIdAsync(int id);
        Task<StatusDto> GetStatusByNameAsync(string Name);
        Task<int> AddStatusAsync(AddStatusDto leaveTtypeDto);
        Task<int> UpdateStatusAsync(UpdateStatusDto leaveTypeDto);
        Task<int> DeactivateStatusAsync(int id);
    }
}
