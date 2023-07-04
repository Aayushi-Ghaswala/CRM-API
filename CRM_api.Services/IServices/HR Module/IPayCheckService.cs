using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IPayCheckService
    {
        Task<ResponseDto<PayCheckDto>> GetPayCheckAsync(string search, SortingParams sortingParams);
        Task<PayCheckDto> GetPayCheckByIdAsync(int id);
        Task<PayCheckDto> GetPayCheckByDesignationAsync(int designationId);
        Task<int> AddPayCheckAsync(AddPayCheckDto payCheckDto);
        Task<int> UpdatePayCheckAsync(UpdatePayCheckDto payCheckDto);
        Task<int> DeactivatePayCheckAsync(int id);
    }
}
