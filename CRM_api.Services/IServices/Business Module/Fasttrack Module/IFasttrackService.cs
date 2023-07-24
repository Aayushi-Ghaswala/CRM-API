using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.IServices.Business_Module.Fasttrack_Module
{
    public interface IFasttrackService
    {
        Task<List<FasttrackBenefitsResponseDto>> GetFasttrackBenefitsAsync();
        Task<List<FasttrackSchemeResponseDto>> GetFasttrackSchemesAsync();
        Task<List<FasttrackLevelCommissionResponseDto>> GetFasttrackLevelCommissionAsync();
        Task<int> AddFasttrackBenefitsAsync(AddFasttrackBenefitsDto addFasttrackBenefits);
        Task<int> UpdateFasttrackBenefitsAsync(UpdateFasttrackBenefitsDto updateFasttrackBenefits);
        Task<int> UpdateFasttrackSchemeAsync(UpdateFasttrackSchemeDto updateFasttrackSchemeDto);
        Task<int> UpdateFasttrackLevelsCommissionAsync(UpdateFasttrackLevelCommissionDto updateFasttrackLevelCommissionDto);
    }
}
