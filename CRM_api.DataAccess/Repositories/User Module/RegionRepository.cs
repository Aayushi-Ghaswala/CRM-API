using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
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
        public async Task<Response<TblCountryMaster>> GetCountries(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblCountryMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblCountryMaster>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblCountryMasters.Where(x => x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var countryResponse = new Response<TblCountryMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return countryResponse;
        }
        #endregion

        #region Get All State Of Country
        public async Task<Response<TblStateMaster>> GetStateBycountry(int countryId, string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblStateMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblStateMaster>(search).Where(x => x.CountryId == countryId && x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblStateMasters.Where(x => x.CountryId == countryId && x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stateResponse = new Response<TblStateMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return stateResponse;
        }
        #endregion

        #region Get City by Name
        public async Task<TblStateMaster> GetStateByName(string? name)
        {
            var state = await _context.TblStateMasters.FirstOrDefaultAsync(x => x.StateName == name);
            return state;
        }
        #endregion

        #region Get All City Of State
        public async Task<Response<TblCityMaster>> GetCityByState(int stateId, string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblCityMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblCityMaster>(search).Where(x => x.StateId == stateId && x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblCityMasters.Where(x => x.StateId == stateId && x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var cityResponse = new Response<TblCityMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return cityResponse;
        }
        #endregion

        #region Get City By Name
        public async Task<TblCityMaster> GetCityByName(string? name)
        {
            var city = await _context.TblCityMasters.FirstOrDefaultAsync(x => x.CityName == name);
            return city;
        }
        #endregion

        #region Add Country
        public async Task<int> AddCountry(TblCountryMaster countryMaster)
        {
            if (_context.TblCountryMasters.Any(x => x.CountryName.ToLower() == countryMaster.CountryName.ToLower() && !x.IsDeleted))
                return 0;

            await _context.TblCountryMasters.AddAsync(countryMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add State
        public async Task<int> AddState(TblStateMaster stateMaster)
        {
            if (_context.TblStateMasters.Any(x => x.StateName.ToLower() == stateMaster.StateName.ToLower() && x.CountryId == stateMaster.StateId && !x.IsDeleted))
                return 0;

            await _context.TblStateMasters.AddAsync(stateMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add City
        public async Task<int> AddCity(TblCityMaster cityMaster)
        {
            if (_context.TblCityMasters.Any(x => x.CityName.ToLower() == cityMaster.CityName.ToLower() && x.StateId == cityMaster.StateId && !x.IsDeleted))
                return 0;

            await _context.TblCityMasters.AddAsync(cityMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Country
        public async Task<int> UpdateCountry(TblCountryMaster countryMaster)
        {
            var country = _context.TblCountryMasters.AsNoTracking().Where(x => x.CountryId == countryMaster.CountryId);

            if (country == null) return 0;

            _context.TblCountryMasters.Update(countryMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update State
        public async Task<int> UpdateState(TblStateMaster stateMaster)
        {
            var state = _context.TblStateMasters.AsNoTracking().Where(x => x.StateId == stateMaster.StateId);

            if (state == null) return 0;

            _context.TblStateMasters.Update(stateMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update City
        public async Task<int> UpdateCity(TblCityMaster cityMaster)
        {
            var city = _context.TblCityMasters.AsNoTracking().Where(x => x.CityId == cityMaster.CityId);

            if (city == null) return 0;

            _context.TblCityMasters.Update(cityMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Country
        public async Task<int> DeactivateCountry(int CountryId)
        {
            var country = await _context.TblCountryMasters.FindAsync(CountryId);

            if (country == null) return 0;

            var states = _context.TblStateMasters.Where(x => x.CountryId == CountryId).ToList();
            foreach (var item in states)
            {
                var cities = _context.TblCityMasters.Where(x => x.StateId == item.StateId).ToList();
                foreach (var city in cities)
                {
                    city.IsDeleted = true;
                }
                item.IsDeleted = true;
            }

            country.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate State
        public async Task<int> DeactivateState(int StateId)
        {
            var states = await _context.TblStateMasters.FindAsync(StateId);

            if (states == null) return 0;

            var cities = _context.TblCityMasters.Where(x => x.StateId == StateId).ToList();
            foreach (var item in cities)
            {
                item.IsDeleted = true;
            }

            states.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactiactivate City
        public async Task<int> DeactivateCity(int CityId)
        {
            var city = await _context.TblCityMasters.FindAsync(CityId);

            if (city == null) return 0;

            city.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
