using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly CRMDbContext _context;

        public RegionRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Countries
        public async Task<IEnumerable<CountryMaster>> GetCountries()
        {
            List<CountryMaster> countries = await _context.CountryMasters.ToListAsync();
            if (countries.Count == 0)
                throw new Exception("No Country Found...");

            return countries;
        }
        #endregion

        #region Get All State Of Country
        public async Task<IEnumerable<StateMaster>> GetStateBycountry(int CountryId)
        {
            List<StateMaster> states = await _context.StateMasters.Where(x => x.Country_Id == CountryId).ToListAsync();
            if (states.Count == 0)
                throw new Exception("No States Found in country...");

            return states;
        }
        #endregion

        #region Get All City Of State
        public async Task<IEnumerable<CityMaster>> GetCityByState(int StateId)
        {
            List<CityMaster> Cities = await _context.CityMasters.Where(x => x.State_Id == StateId).ToListAsync();
            if (Cities.Count == 0)
                throw new Exception("No Cities Found in state...");

            return Cities;
        }
        #endregion
    }
}
