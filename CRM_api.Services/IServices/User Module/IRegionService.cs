using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IRegionService
    {
        Task<ResponseDto<CountryMasterDto>> GetCountriesAsync(int page);
        Task<ResponseDto<StateMasterDto>> GetstateByCountry(int countryId, int page);
        Task<ResponseDto<CityMasterDto>> GetCityByState(int stateId, int page);
    }
}
