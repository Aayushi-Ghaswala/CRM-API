using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IRegionRepository
    {
        Task<IEnumerable<TblCountryMaster>> GetCountries();
        Task<IEnumerable<TblStateMaster>> GetStateBycountry(int CountryId);
        Task<IEnumerable<TblCityMaster>> GetCityByState(int StateId);
    }
}
