using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
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
        public async Task<Response<TblCountryMaster>> GetCountries(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblCountryMasters.Count() / pageResult);

            List<TblCountryMaster> countries = await _context.TblCountryMasters.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var countryResponse = new Response<TblCountryMaster>()
            {
                Values = countries,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return countryResponse;
        }
        #endregion

        #region Get All State Of Country
        public async Task<Response<TblStateMaster>> GetStateBycountry(int CountryId, int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblStateMasters.Where(x => x.CountryId == CountryId).Count() / pageResult);

            List<TblStateMaster> states = await _context.TblStateMasters.Where(x => x.CountryId == CountryId).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var stateResponse = new Response<TblStateMaster>()
            {
                Values = states,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return stateResponse;
        }
        #endregion

        #region Get All City Of State
        public async Task<Response<TblCityMaster>> GetCityByState(int StateId, int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblCityMasters.Where(x => x.StateId == StateId).Count() / pageResult);

            List<TblCityMaster> cities = await _context.TblCityMasters.Where(x => x.StateId == StateId).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var cityResponse = new Response<TblCityMaster>()
            {
                Values = cities,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return cityResponse;
        }
        #endregion
    }
}
