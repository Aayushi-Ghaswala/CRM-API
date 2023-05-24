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
        public async Task<Response<TblCountryMaster>> GetCountries(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblCountryMasters.Where(x => x.IsDeleted != true).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblCountryMaster>(searchingParams);
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
        public async Task<Response<TblStateMaster>> GetStateBycountry(int countryId, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblStateMasters.Where(x => x.CountryId == countryId && x.IsDeleted != true).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblStateMaster>(searchingParams);
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

        #region Get All City Of State
        public async Task<Response<TblCityMaster>> GetCityByState(int stateId, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblCityMasters.Where(x => x.StateId == stateId && x.IsDeleted != true).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblCityMaster>(searchingParams);
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

        #region Deactivate Country
        public async Task<int> DeactivateCountry(int CountryId)
        {
            var country = await _context.TblCountryMasters.FindAsync(CountryId);

            if(country == null) return 0;

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

            if(states == null) return 0;

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
