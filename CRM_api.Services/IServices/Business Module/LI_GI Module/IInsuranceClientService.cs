using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.LI_GI_Module
{
    public interface IInsuranceClientService
    {
        Task<ResponseDto<InsuranceClientDto>> GetInsuranceClientsAsync(string? filterString, string search, SortingParams sortingParams);
        Task<ResponseDto<InsuranceCompanyListDto>> GetCompanyListByInsTypeIdAsync(int id, SortingParams sortingParams);
        Task<InsuranceClientDto> GetInsuranceClientByIdAsync(int id);
        Task<(string, int)> ImportInsClientsFileAsync(IFormFile formFile);
        Task<int> AddInsuranceClientAsync(AddInsuranceClientDto insuranceClientDto);
        Task<int> UpdateInsuranceClientAsync(UpdateInsuranceClientDto insuranceClientDto);
        Task<int> DeactivateInsClientAsync(int id);
    }
}
