using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.MGain_Module
{
    public interface IMGainSchemeService
    {
        Task<ResponseDto<MGainSchemeDto>> GetMGainSchemeDetailsAsync(bool? IsCumulative, string? searchingParams, SortingParams sortingParams);
        Task<int> AddMGainSchemeAsync(AddMGainSchemeDto addMGainSchemeDto);
        Task<int> UpdateMGainSchemeAsync(UpdateMGainSchemeDto updateMGainSchemeDto);
    }
}
