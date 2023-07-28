using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class SourceRepository : ISourceRepository
    {
        private readonly CRMDbContext _context;

        public SourceRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Sources
        public async Task<Response<TblSourceMaster>> GetSources(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblSourceMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblSourceMaster>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblSourceMasters.Where(x => x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var sourceResponse = new Response<TblSourceMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return sourceResponse;
        }
        #endregion

        #region Get Source by Id
        public async Task<TblSourceMaster> GetSourceById(int id)
        {
            var source = await _context.TblSourceMasters.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return source;
        }
        #endregion

        #region Get Source by Name
        public async Task<TblSourceMaster> GetSourceByName(string Name)
        {
            var source = await _context.TblSourceMasters.FirstAsync(x => x.Name.ToLower().Contains(Name.ToLower()) && x.IsDeleted != true);
            return source;
        }
        #endregion

        #region Add Source
        public async Task<int> AddSource(TblSourceMaster source)
        {
            if (_context.TblSourceMasters.Any(x => x.Name == source.Name))
                return 0;

            _context.TblSourceMasters.Add(source);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Source
        public async Task<int> UpdateSource(TblSourceMaster source)
        {
            var sources = _context.TblSourceMasters.AsNoTracking().Where(x => x.Id == source.Id);

            if (sources == null) return 0;

            _context.TblSourceMasters.Update(source);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Source
        public async Task<int> DeactivateSource(int id)
        {
            var source = await _context.TblSourceMasters.FindAsync(id);

            if(source == null) return 0;

            source.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}