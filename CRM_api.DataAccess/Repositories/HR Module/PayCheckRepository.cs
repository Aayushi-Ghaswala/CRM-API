using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class PayCheckRepository : IPayCheckRepository
    {
        private readonly CRMDbContext _context;

        public PayCheckRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get PayChecks
        public async Task<Response<TblPayCheck>> GetPayChecks(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblPayChecks.Where(x => x.IsDeleted != true).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblPayCheck>(search).Where(x => x.IsDeleted != true).AsQueryable(); ;
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var payCheckResponse = new Response<TblPayCheck>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return payCheckResponse;
        }
        #endregion

        #region Get PayCheck by Designation
        public async Task<TblPayCheck> GetPayChecksByDesignation(int designationId)
        {
            var payCheck = await _context.TblPayChecks.FirstAsync(x => x.DesignationId == designationId && x.IsDeleted != true);
            return payCheck;
        }
        #endregion

        #region Get PayCheck by Id
        public async Task<TblPayCheck> GetPayCheckById(int id)
        {
            var payCheck = await _context.TblPayChecks.FirstAsync(x => x.PayCheckId == id && x.IsDeleted != true);
            return payCheck;
        }
        #endregion

        #region Add PayCheck
        public async Task<int> AddPayCheck(TblPayCheck payCheckMaster)
        {
            if (_context.TblPayChecks.Any(x => x.DesignationId == payCheckMaster.DesignationId))
                return 0;

            _context.TblPayChecks.Add(payCheckMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update PayCheck
        public async Task<int> UpdatePayCheck(TblPayCheck payCheckMaster)
        {
            var payCheck = _context.TblPayChecks.AsNoTracking().Where(x => x.PayCheckId == payCheckMaster.PayCheckId);

            if (payCheck == null) return 0;

            _context.TblPayChecks.Update(payCheckMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate PayCheck
        public async Task<int> DeactivatePayCheck(int id)
        {
            var payCheck = await _context.TblPayChecks.FindAsync(id);

            if (payCheck == null) return 0;

            payCheck.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

    }
}
