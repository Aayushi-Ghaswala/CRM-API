using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.MGain_Module;

namespace CRM_api.Services.Services.Business_Module.MGain_Module
{
    public class MGainSchemeService : IMGainSchemeService
    {
        private readonly IMGainSchemeRepository _mGainSchemeRepository;
        private readonly IMapper _mapper;

        public MGainSchemeService(IMGainSchemeRepository mGainSchemeRepository, IMapper mapper)
        {
            _mGainSchemeRepository = mGainSchemeRepository;
            _mapper = mapper;
        }

        #region Get All MGain Scheme
        public async Task<ResponseDto<MGainSchemeDto>> GetMGainSchemeDetailsAsync(bool? IsActive, string? searchingParams, SortingParams sortingParams)
        {
            var mGainScheme = await _mGainSchemeRepository.GetMGainSchemeDetails(IsActive, searchingParams, sortingParams);
            var mapMGainScheme = _mapper.Map<ResponseDto<MGainSchemeDto>>(mGainScheme);
            return mapMGainScheme;
        }
        #endregion

        #region Add MGain Scheme
        public async Task<int> AddMGainSchemeAsync(AddMGainSchemeDto addMGainSchemeDto)
        {
            var addMGainScheme = _mapper.Map<TblMgainSchemeMaster>(addMGainSchemeDto);
            return await _mGainSchemeRepository.AddMGainScheme(addMGainScheme);
        }
        #endregion

        #region Update MGain Scheme
        public async Task<int> UpdateMGainSchemeAsync(UpdateMGainSchemeDto updateMGainSchemeDto)
        {
            var updateMGainScheme = _mapper.Map<TblMgainSchemeMaster>(updateMGainSchemeDto);
            return await _mGainSchemeRepository.UpdateMGainScheme(updateMGainScheme);
        }
        #endregion
    }
}
