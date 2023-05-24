using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRegionRepository
    {
        Task<Response<TblCountryMaster>> GetCountries(string search, SortingParams sortingParams);
        Task<Response<TblStateMaster>> GetStateBycountry(int countryId, string search, SortingParams sortingParams);
        Task<Response<TblCityMaster>> GetCityByState(int stateId, string search, SortingParams sortingParams);
        Task<int> DeactivateCountry(int CountryId);
        Task<int> DeactivateState(int StateId);
        Task<int> DeactivateCity(int CityId);
    }
}
