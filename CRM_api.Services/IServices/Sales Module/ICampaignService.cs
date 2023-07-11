using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ICampaignService
    {
        Task<ResponseDto<CampaignDto>> GetCampaignsAsync(string search, SortingParams sortingParams);
        Task<CampaignDto> GetCampaignByIdAsync(int id);
        Task<CampaignDto> GetCampaignByNameAsync(string Name);
        Task<int> AddCampaignAsync(AddCampaignDto campaignDto);
        Task<int> UpdateCampaignAsync(UpdateCampaignDto campaignDto);
        Task<int> DeactivateCampaignAsync(int id);
    }
}
