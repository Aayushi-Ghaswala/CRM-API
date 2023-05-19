using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IRegionService
    {
        Task<ResponseDto<CountryMasterDto>> GetCountriesAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<ResponseDto<StateMasterDto>> GetstateByCountry(int countryId, Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<ResponseDto<CityMasterDto>> GetCityByState(int stateId, Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<int> DeactivateCountryAsync(int countryId);
        Task<int> DeactivateStateAsync(int stateId);
        Task<int> DeactivateCityAsync(int cityId);
    }
}
