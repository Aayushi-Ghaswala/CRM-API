using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;

namespace CRM_api.Services.IServices.Business_Module.Fasttrack_Module
{
    public interface IFasttrackService
    {
        Task<int> UpdateFasttrackSchemeAsync(UpdateFasttrackSchemeDto updateFasttrackSchemeDto);
        Task<int> UpdateFasttrackLevelsCommissionAsync(UpdateFasttrackLevelCommissionDto updateFasttrackLevelCommissionDto);
    }
}
