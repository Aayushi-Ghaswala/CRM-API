using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
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
        public async Task<ResponseDto<CountryMasterDto>> GetCountriesAsync(string search, SortingParams sortingParams)
        {
            var countries = await _regionRepository.GetCountries(search, sortingParams);
            var mapCountries = _mapper.Map<ResponseDto<CountryMasterDto>>(countries);

            return mapCountries;
        }
        #endregion

        #region Get All State By Country
        public async Task<ResponseDto<StateMasterDto>> GetstateByCountryAsync(int countryId, string search, SortingParams sortingParams)
        {
            var states = await _regionRepository.GetStateBycountry(countryId, search, sortingParams);
            var mapStates = _mapper.Map<ResponseDto<StateMasterDto>>(states);

            return mapStates;
        }
        #endregion

        #region Get All Cities By State
        public async Task<ResponseDto<CityMasterDto>> GetCityByStateAsync(int stateId, string search, SortingParams sortingParams)
        {
            var cities = await _regionRepository.GetCityByState(stateId, search, sortingParams);
            var mapCities = _mapper.Map<ResponseDto<CityMasterDto>>(cities);

            return mapCities;
        }
        #endregion

        #region Add Country
        public async Task<int> AddCountryAsync(AddCountryDto countryDto)
        {
            var country = _mapper.Map<TblCountryMaster>(countryDto);

            return await _regionRepository.AddCountry(country);
        }
        #endregion

        #region Add State
        public async Task<int> AddStateAsync(AddStateDto stateDto)
        {
            var state = _mapper.Map<TblStateMaster>(stateDto);

            return await _regionRepository.AddState(state);
        }
        #endregion

        #region Add City
        public async Task<int> AddCityAsync(AddCityDto cityDto)
        {
            var city = _mapper.Map<TblCityMaster>(cityDto);

            return await _regionRepository.AddCity(city);
        }
        #endregion

        #region Update Country
        public async Task<int> UpdateCountryAsync(UpdateCountryDto countryDto)
        {
            var country = _mapper.Map<TblCountryMaster>(countryDto);

            return await _regionRepository.UpdateCountry(country);
        }
        #endregion

        #region Update State
        public async Task<int> UpdateStateAsync(UpdateStateDto stateDto)
        {
            var state = _mapper.Map<TblStateMaster>(stateDto);

            return await _regionRepository.UpdateState(state);
        }
        #endregion

        #region Update City
        public async Task<int> UpdateCityAsync(UpdateCityDto cityDto)
        {
            var city = _mapper.Map<TblCityMaster>(cityDto);

            return await _regionRepository.UpdateCity(city);
        }
        #endregion

        #region Deactivate Country
        public async Task<int> DeactivateCountryAsync(int countryId)
        {
            return await _regionRepository.DeactivateCountry(countryId);
        }
        #endregion

        #region Deactivate State
        public async Task<int> DeactivateStateAsync(int stateId)
        {
            return await _regionRepository.DeactivateState(stateId);
        }
        #endregion

        #region Deactivate City
        public async Task<int> DeactivateCityAsync(int cityId)
        {
            return await _regionRepository.DeactivateCity(cityId);
        }
        #endregion
    }
}
