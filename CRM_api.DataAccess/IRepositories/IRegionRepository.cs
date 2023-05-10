using CRM_api.DataAccess.Model;

namespace CRM_api.DataAccess.IRepositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<CountryMaster>> GetCountries();
        Task<IEnumerable<StateMaster>> GetStateBycountry(int CountryId);
        Task<IEnumerable<CityMaster>> GetCityByState(int StateId);
    }
}
