using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.User_Module
{
    public class RegionRepository : IRegionRepository
    {
        private readonly CRMDbContext _context;

        public RegionRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Countries
        public async Task<IEnumerable<TblCountryMaster>> GetCountries()
        {
            List<TblCountryMaster> countries = await _context.TblCountryMasters.ToListAsync();
            if (countries.Count == 0)
                throw new Exception("No Country Found...");

            return countries;
        }
        #endregion

        #region Get All State Of Country
        public async Task<IEnumerable<TblStateMaster>> GetStateBycountry(int CountryId)
        {
            List<TblStateMaster> states = await _context.TblStateMasters.Where(x => x.CountryId == CountryId).ToListAsync();
            if (states.Count == 0)
                throw new Exception("No States Found in country...");

            return states;
        }
        #endregion

        #region Get All City Of State
        public async Task<IEnumerable<TblCityMaster>> GetCityByState(int StateId)
        {
            List<TblCityMaster> Cities = await _context.TblCityMasters.Where(x => x.StateId == StateId).ToListAsync();
            if (Cities.Count == 0)
                throw new Exception("No Cities Found in state...");

            return Cities;
        }
        #endregion
    }
}
