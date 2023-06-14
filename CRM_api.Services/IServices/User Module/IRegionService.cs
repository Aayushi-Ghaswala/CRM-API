using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IRegionService
    {
        Task<ResponseDto<CountryMasterDto>> GetCountriesAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<StateMasterDto>> GetstateByCountryAsync(int countryId, string search, SortingParams sortingParams);
        Task<ResponseDto<CityMasterDto>> GetCityByStateAsync(int stateId, string search, SortingParams sortingParams);
        Task<int> AddCountryAsync(AddCountryDto countryDto);
        Task<int> AddStateAsync(AddStateDto stateDto);
        Task<int> AddCityAsync(AddCityDto cityDto);
        Task<int> UpdateCountryAsync(UpdateCountryDto countryDto);
        Task<int> UpdateStateAsync(UpdateStateDto stateDto);
        Task<int> UpdateCityAsync(UpdateCityDto cityDto);
        Task<int> DeactivateCountryAsync(int countryId);
        Task<int> DeactivateStateAsync(int stateId);
        Task<int> DeactivateCityAsync(int cityId);
    }
}
