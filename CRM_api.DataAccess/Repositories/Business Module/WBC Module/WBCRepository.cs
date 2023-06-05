using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.WBC_Module
{
    public class WBCRepository : IWBCRepository
    {
        private readonly CRMDbContext _context;

        public WBCRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get all Wbc scheme types
        public async Task<Response<TblWbcTypeMaster>> GetAllWbcSchemeTypes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblWbcTypeMasters.AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblWbcTypeMaster>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var wbcSchemeTypesResponse = new Response<TblWbcTypeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return wbcSchemeTypesResponse;
        }
        #endregion

        #region Get all subInvestment types
        public async Task<Response<TblSubInvesmentType>> GetAllSubInvestmentTypes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblSubInvesmentTypes.AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblSubInvesmentType>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subInvestmentTypesResponse = new Response<TblSubInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return subInvestmentTypesResponse;
        }
        #endregion

        #region Get all subSubInvestment types
        public async Task<Response<TblSubsubInvType>> GetAllSubSubInvestmentTypes(string? searchingParams, SortingParams sortingParams, int? subInvestmentTypeId)
        {
            double pageCount = 0;
            List<TblSubsubInvType> data = new List<TblSubsubInvType>();
            IQueryable<TblSubsubInvType> filterData = data.AsQueryable();

            if (subInvestmentTypeId != null) 
                filterData = _context.TblSubsubInvTypes.Include(s => s.TblSubInvesmentType).Where(s => s.SubInvTypeId == subInvestmentTypeId).AsQueryable();
            else
                filterData = _context.TblSubsubInvTypes.Include(s => s.TblSubInvesmentType).AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblSubsubInvType>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subSubInvestmentTypesResponse = new Response<TblSubsubInvType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return subSubInvestmentTypesResponse;
        }
        #endregion

        #region Get All WBC Schemes
        public async Task<Response<TblWbcSchemeMaster>> GetAllWbcSchemes(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblWbcSchemeMasters.Include(w => w.TblWbcTypeMaster).Include(s => s.TblSubInvesmentType).Include(w => w.TblSubsubInvType).AsQueryable();

            if (searchingParams != null)
            {
                filterData = _context.Search<TblWbcSchemeMaster>(searchingParams).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var wbcSchemeResponse = new Response<TblWbcSchemeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return wbcSchemeResponse;
        }
        #endregion

        #region Add WBC Scheme
        public async Task<int> AddWbcScheme(TblWbcSchemeMaster wbcSchemeMaster)
        {
            _context.TblWbcSchemeMasters.Add(wbcSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update WBC Scheme
        public Task<int> UpdateWbcScheme(TblWbcSchemeMaster wbcSchemeMaster)
        {
            _context.TblWbcSchemeMasters.Update(wbcSchemeMaster);
            return _context.SaveChangesAsync();
        }
        #endregion

        #region Delete WBC Scheme
        public async Task<int> DeleteWbcScheme(int id)
        {
            var scheme = await _context.TblWbcSchemeMasters.FindAsync(id);
            if (scheme == null) return 0;

            _context.TblWbcSchemeMasters.Remove(scheme);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
