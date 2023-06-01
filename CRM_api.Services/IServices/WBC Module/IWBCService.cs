using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.WBC_Module
{
    public interface IWBCService
    {
        Task<ResponseDto<WbcTypeDto>> GetAllWbcSchemeTypesAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SubInvestmentTypeDto>> GetAllSubInvestmentTypesAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SubSubInvestmentTypeDto>> GetAllSubSubInvestmentTypesAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<WBCSchemeMasterDto>> GetAllWbcSchemesAsync(string? searchingParams, SortingParams sortingParams);
        Task<int> AddWbcSchemeAsync(AddWBCSchemeDto addWBCSchemeDto);
        Task<int> UpdateWbcSchemeAsync(UpdateWBCSchemeDto updateWBCSchemeDto);
        Task<int> DeleteWbcSchemeAsync(int id);
    }
}
