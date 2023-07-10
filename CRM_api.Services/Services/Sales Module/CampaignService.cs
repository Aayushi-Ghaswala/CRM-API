using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;

namespace CRM_api.Services.Services.Sales_Module
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly IMapper _mapper;

        public CampaignService(ICampaignRepository campaignRepository, IMapper mapper)
        {
            _campaignRepository = campaignRepository;
            _mapper = mapper;
        }

        #region Get Campaigns
        public async Task<ResponseDto<CampaignDto>> GetCampaignsAsync(string search, SortingParams sortingParams)
        {
            var campaigns = await _campaignRepository.GetCampaigns(search,sortingParams);
            var mapCampaign = _mapper.Map<ResponseDto<CampaignDto>>(campaigns);
            return mapCampaign;
        }
        #endregion

        #region Get Campaign By Id
        public async Task<CampaignDto> GetCampaignByIdAsync(int id)
        {
            var campaign = await _campaignRepository.GetCampaignById(id);
            var mappedCampaign = _mapper.Map<CampaignDto>(campaign);
            return mappedCampaign;
        }
        #endregion

        #region Get Campaign By Name
        public async Task<CampaignDto> GetCampaignByNameAsync(string Name)
        {
            var campaign = await _campaignRepository.GetCampaignByName(Name);
            var mappedCampaign = _mapper.Map<CampaignDto>(campaign);
            return mappedCampaign;
        }
        #endregion

        #region Add Campaign
        public async Task<int> AddCampaignAsync(AddCampaignDto campaignDto)
        {
            var campaign = _mapper.Map<TblCampaignMaster>(campaignDto);
            return await _campaignRepository.AddCampaign(campaign);
        }
        #endregion

        #region Update Campaign
        public async Task<int> UpdateCampaignAsync(UpdateCampaignDto campaignDto)
        {
            var campaign = _mapper.Map<TblCampaignMaster>(campaignDto);
            return await _campaignRepository.UpdateCampaign(campaign);
        }
        #endregion

        #region Deactivate Campaign
        public async Task<int> DeactivateCampaignAsync(int id)
        {
            return await _campaignRepository.DeactivateCampaign(id);
        }
        #endregion
    }
}
