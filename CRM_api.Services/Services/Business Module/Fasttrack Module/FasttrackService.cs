using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
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
