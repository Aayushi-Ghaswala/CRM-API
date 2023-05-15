using AutoMapper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;

namespace CRM_api.Services.Services.User_Module
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionService(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        #region Get All Countries
        public async Task<ResponseDto<CountryMasterDto>> GetCountriesAsync(int page)
        {
            var countries = await _regionRepository.GetCountries(page);
            var mapCountries = _mapper.Map<ResponseDto<CountryMasterDto>>(countries);

            return mapCountries;
        }
        #endregion

        #region Get All State By Country
        public async Task<ResponseDto<StateMasterDto>> GetstateByCountry(int countryId, int page)
        {
            var states = await _regionRepository.GetStateBycountry(countryId, page);
            var mapStates = _mapper.Map<ResponseDto<StateMasterDto>>(states);

            return mapStates;
        }
        #endregion

        #region Get All Cities By State
        public async Task<ResponseDto<CityMasterDto>> GetCityByState(int stateId, int page)
        {
            var cities = await _regionRepository.GetCityByState(stateId, page);
            var mapCities = _mapper.Map<ResponseDto<CityMasterDto>>(cities);

            return mapCities;
        }
        #endregion

        #region Deactivate Country
        public async Task<int> DeactivateCountryAsync(int CountryId)
        {
            return await _regionRepository.DeactivateCountry(CountryId);
        }
        #endregion

        #region Deactivate State
        public async Task<int> DeactivateStateAsync(int StateId)
        {
            return await _regionRepository.DeactivateState(StateId);
        }
        #endregion

        #region Deactivate City
        public async Task<int> DeactivateCityAsync(int CityId)
        {
            return await _regionRepository.DeactivateCity(CityId);
        }
        #endregion
    }
}
