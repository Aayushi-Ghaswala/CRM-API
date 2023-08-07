using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class SourceTypeRepository : ISourceTypeRepository
    {
        private readonly CRMDbContext _context;

        public SourceTypeRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Source Types
        public async Task<Response<TblSourceTypeMaster>> GetSourceTypes(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblSourceTypeMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblSourceTypeMaster>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblSourceTypeMasters.Where(x => x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var sourceTypeResponse = new Response<TblSourceTypeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return sourceTypeResponse;
        }
        #endregion

        #region Get SourceType by Id
        public async Task<TblSourceTypeMaster> GetSourceTypeById(int id)
        {
            var status = await _context.TblSourceTypeMasters.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return status;
        }
        #endregion

        #region Get SourceType by Name
        public async Task<TblSourceTypeMaster> GetSourceTypeByName(string Name)
        {
            var status = await _context.TblSourceTypeMasters.FirstAsync(x => x.Name.ToLower().Contains(Name.ToLower()) && x.IsDeleted != true);
            return status;
        }
        #endregion

        #region Add SourceType
        public async Task<int> AddSourceType(TblSourceTypeMaster status)
        {
            if (_context.TblSourceTypeMasters.Any(x => x.Name == status.Name))
                return 0;

            _context.TblSourceTypeMasters.Add(status);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update SourceType
        public async Task<int> UpdateSourceType(TblSourceTypeMaster status)
        {
            var sourceTypes = _context.TblSourceTypeMasters.AsNoTracking().Where(x => x.Id == status.Id);

            if (sourceTypes == null) return 0;

            _context.TblSourceTypeMasters.Update(status);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate SourceType
        public async Task<int> DeactivateSourceType(int id)
        {
            var status = await _context.TblSourceTypeMasters.FindAsync(id);

            if (status == null) return 0;

            status.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}