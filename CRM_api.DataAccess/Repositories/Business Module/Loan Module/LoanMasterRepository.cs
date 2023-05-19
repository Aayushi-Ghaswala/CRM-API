using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Loan_Module
{
    public class LoanMasterRepository : ILoanMasterRepository
    {
        private readonly CRMDbContext _context;

        public LoanMasterRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Loan Details
        public async Task<Response<TblLoanMaster>> GetLoanDetails(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblLoanMasters.AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblLoanMaster>(searchingParams);
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var loanResponse = new Response<TblLoanMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return loanResponse;
        }
        #endregion

        #region Get Loan Detail By Id
        public async Task<TblLoanMaster> GetLoanDetailById(int id)
        {
            var loanDetail = await _context.TblLoanMasters.Where(x => x.Id == id).Include(u => u.TblUserMaster).Include(c => c.TblUserCategoryMaster)
                                                            .FirstOrDefaultAsync();

            return loanDetail;
        }
        #endregion

        #region Add Loan Detail
        public async Task<int> AddLoanDetail(TblLoanMaster tblLoan)
        {
            _context.TblLoanMasters.Add(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Loan Detail
        public async Task<int> UpdateLoanDetail(TblLoanMaster tblLoan)
        {
            var loan = await _context.TblLoanMasters.FindAsync(tblLoan.Id);

            if (loan == null) return 0;

            _context.TblLoanMasters.Update(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Loan Detail
        public async Task<int> DeactivateLoanDetail(int id)
        {
            var loan = await _context.TblLoanMasters.FindAsync(id);

            if (loan == null) return 0;

            loan.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
