using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            var loanDetail = await _context.TblLoanMasters.Where(x => x.Id == id).Include(u => u.TblUserMaster).Include(c => c.TblLoanTypeMaster)
                                                            .Include(x => x.TblBankMaster).FirstOrDefaultAsync();

            return loanDetail;
        }
        #endregion

        #region Add Loan Detail
        public async Task<int> AddLoanDetail(TblLoanMaster tblLoan)
        {
            if (_context.TblLoanMasters.Any(x => x.UserId == tblLoan.UserId && x.LoanTypeId == tblLoan.LoanTypeId && x.IsCompleted == false && x.IsDeleted == false))
                return 0;

            _context.TblLoanMasters.Add(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Loan Detail
        public async Task<int> UpdateLoanDetail(TblLoanMaster tblLoan)
        {
            var loan = _context.TblLoanMasters.AsNoTracking().Where(x => x.Id == tblLoan.Id);

            if (loan == null) return 0;

            _context.TblLoanMasters.Update(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get All Bank Details
        public async Task<Response<TblBankMaster>> GetLBankDetails(int page)
        {
            float pageResult = 10f;
            double pageCount = Math.Ceiling(_context.TblBankMasters.Count() / pageResult);

            var bankDetails = await _context.TblBankMasters.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var bankDetailsResponse = new Response<TblBankMaster>()
            {
                Values = bankDetails,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return bankDetailsResponse;
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
