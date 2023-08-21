using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRegionRepository
    {
        Task<Response<TblCountryMaster>> GetCountries(string search, SortingParams sortingParams);
        Task<Response<TblStateMaster>> GetStateBycountry(int countryId, string search, SortingParams sortingParams);
        Task<TblStateMaster> GetStateByName(string? name);
        Task<Response<TblCityMaster>> GetCityByState(int stateId, string search, SortingParams sortingParams);
        Task<TblCityMaster> GetCityByName(string? name);
        Task<int> AddCountry(TblCountryMaster countryMaster);
        Task<int> AddState(TblStateMaster stateMaster);
        Task<int> AddCity(TblCityMaster cityMaster);
        Task<int> UpdateCountry(TblCountryMaster countryMaster);
        Task<int> UpdateState(TblStateMaster stateMaster);
        Task<int> UpdateCity(TblCityMaster cityMaster);
        Task<int> DeactivateCountry(int CountryId);
        Task<int> DeactivateState(int StateId);
        Task<int> DeactivateCity(int CityId);
    }
}
