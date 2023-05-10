using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.IServices
{
    public interface IRegionService
    {
        Task<List<CountryMasterDto>> GetCountriesAsync();
        Task<List<StateMasterDto>> GetstateByCountry(int countryId);
        Task<List<CityMasterDto>> GetCityByState(int stateId);
    }
}
