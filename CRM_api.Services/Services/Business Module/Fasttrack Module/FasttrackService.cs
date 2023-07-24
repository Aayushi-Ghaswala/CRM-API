using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.IServices.Business_Module.Fasttrack_Module;

namespace CRM_api.Services.Services.Business_Module.Fasttrack_Module
{
    public class FasttrackService : IFasttrackService
    {
        private readonly IFasttrackRepository _fasttrackRepository;
        private readonly IMapper _mapper;

        public FasttrackService(IFasttrackRepository fasttrackRepository, IMapper mapper)
        {
            _fasttrackRepository = fasttrackRepository;
            _mapper = mapper;
        }
        #region Get Fasttrack Benefits
        public async Task<List<FasttrackBenefitsResponseDto>> GetFasttrackBenefitsAsync()
        {
            var fasttrackBenefits = await _fasttrackRepository.GetFasttrackBenefits();
            return _mapper.Map<List<FasttrackBenefitsResponseDto>>(fasttrackBenefits);
        }
                #endregion

        #region Get Fasttrack Schemes
        public async Task<List<FasttrackSchemeResponseDto>> GetFasttrackSchemesAsync()
        {
            var fasttrackSchemes = await _fasttrackRepository.GetFasttrackSchemes();
            return _mapper.Map<List<FasttrackSchemeResponseDto>>(fasttrackSchemes);
        }
                #endregion

        #region Get Fasttrack Level Commissions
        public async Task<List<FasttrackLevelCommissionResponseDto>> GetFasttrackLevelCommissionAsync()
        {
            var fasttrackLevelCommission = await _fasttrackRepository.GetFasttrackLevelCommission();
            return _mapper.Map<List<FasttrackLevelCommissionResponseDto>>(fasttrackLevelCommission);
        }
                #endregion

        #region Add Fasttrack Benefits
        public async Task<int> AddFasttrackBenefitsAsync(AddFasttrackBenefitsDto addFasttrackBenefits)
        {
            var mapFasttrackBenefits = _mapper.Map<TblFasttrackBenefits>(addFasttrackBenefits);
            return await _fasttrackRepository.AddFasttrackBenefit(mapFasttrackBenefits);
        }
                #endregion

        #region Add Fasttrack Benefits
        public async Task<int> UpdateFasttrackBenefitsAsync(UpdateFasttrackBenefitsDto updateFasttrackBenefits)
        {
            var mapFasttrackBenefits = _mapper.Map<TblFasttrackBenefits>(updateFasttrackBenefits);
            return await _fasttrackRepository.UpdateFasttrackBenefit(mapFasttrackBenefits);
        }
                #endregion

        #region Update fasttrack scheme
        public async Task<int> UpdateFasttrackSchemeAsync(UpdateFasttrackSchemeDto updateFasttrackSchemeDto)
        {
            var scheme = _mapper.Map<TblFasttrackSchemeMaster>(updateFasttrackSchemeDto);
            return await _fasttrackRepository.UpdateFasttrackScheme(scheme);
        }
        #endregion

        #region Update fasttrack levels commission
        public async Task<int> UpdateFasttrackLevelsCommissionAsync(UpdateFasttrackLevelCommissionDto updateFasttrackLevelCommissionDto)
        {
            var levelCommission = _mapper.Map<TblFasttrackLevelCommission>(updateFasttrackLevelCommissionDto);
            return await _fasttrackRepository.UpdateFasttrackLevelsCommission(levelCommission);
        }
        #endregion
    }
}
