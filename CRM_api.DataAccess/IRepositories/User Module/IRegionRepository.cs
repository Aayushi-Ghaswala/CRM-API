using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRegionRepository
    {
        Task<Response<TblCountryMaster>> GetCountries(int page);
        Task<Response<TblStateMaster>> GetStateBycountry(int CountryId, int page);
        Task<Response<TblCityMaster>> GetCityByState(int StateId, int page);
        Task<int> DeactivateCountry(int CountryId);
        Task<int> DeactivateState(int StateId);
        Task<int> DeactivateCity(int CityId);
    }
}
