using AutoMapper;
using CRM_api.DataAccess.IRepositories;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.IServices;

namespace CRM_api.Services.Services
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
        public async Task<List<CountryMasterDto>> GetCountriesAsync()
        {
            var countries = await _regionRepository.GetCountries();
            var mapCountries = _mapper.Map<List<CountryMasterDto>>(countries);

            return mapCountries;
        }
        #endregion

        #region Get All State By Country
        public async Task<List<StateMasterDto>> GetstateByCountry(int countryId)
        {
            var states = await _regionRepository.GetStateBycountry(countryId);
            var mapStates = _mapper.Map<List<StateMasterDto>>(states);

            return mapStates;
        }
        #endregion

        #region Get All Cities By State
        public async Task<List<CityMasterDto>> GetCityByState(int stateId)
        {
            var cities = await _regionRepository.GetCityByState(stateId);
            var mapCities = _mapper.Map<List<CityMasterDto>>(cities);

            return mapCities;
        }
        #endregion
    }
}
