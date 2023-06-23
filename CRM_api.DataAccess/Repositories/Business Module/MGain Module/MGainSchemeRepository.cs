using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.MGain_Module
{
    public class MGainSchemeRepository : IMGainSchemeRepository
    {
        private readonly CRMDbContext _context;

        public MGainSchemeRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All MGain Scheme
        public async Task<Response<TblMgainSchemeMaster>> GetMGainSchemeDetails(bool? IsActive, string? searchingParamas, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblMgainSchemeMaster> tblMgainSchemes = new List<TblMgainSchemeMaster>();
            IQueryable<TblMgainSchemeMaster> mGainScheme = tblMgainSchemes.AsQueryable();

            if (IsActive is true)
                mGainScheme = _context.TblMgainSchemeMasters.Where(x => x.IsActive == true).AsQueryable();
            else if (IsActive is false)
                mGainScheme = _context.TblMgainSchemeMasters.Where(x => x.IsActive == false).AsQueryable();
            else
                mGainScheme = _context.TblMgainSchemeMasters.AsQueryable();

            if (searchingParamas != null)
            {
                if (IsActive is true)
                    mGainScheme = _context.Search<TblMgainSchemeMaster>(searchingParamas).Where(x => x.IsActive == true).AsQueryable();
                else if (IsActive is false)
                    mGainScheme = _context.Search<TblMgainSchemeMaster>(searchingParamas).Where(x => x.IsActive == false).AsQueryable();
                else
                    mGainScheme = _context.Search<TblMgainSchemeMaster>(searchingParamas);
            }
            pageCount = Math.Ceiling(mGainScheme.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(mGainScheme, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mGainResponse = new Response<TblMgainSchemeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return mGainResponse;
        }
        #endregion

        #region Get MGain Shceme By Id
        public async Task<TblMgainSchemeMaster> GetMGainSchemeById(int id)
        {
            var mgainShceme = await _context.TblMgainSchemeMasters.Where(x => x.Id == id).FirstAsync();

            return mgainShceme;
        }
        #endregion

        #region Add MGain Scheme
        public async Task<int> AddMGainScheme(TblMgainSchemeMaster tblMgainSchemeMaster)
        {
            if (_context.TblMgainSchemeMasters.Any(x => x.Schemename == tblMgainSchemeMaster.Schemename))
                return 0;

            _context.TblMgainSchemeMasters.Add(tblMgainSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update MGain Scheme
        public async Task<int> UpdateMGainScheme(TblMgainSchemeMaster tblMgainSchemeMaster)
        {
            _context.TblMgainSchemeMasters.Update(tblMgainSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
