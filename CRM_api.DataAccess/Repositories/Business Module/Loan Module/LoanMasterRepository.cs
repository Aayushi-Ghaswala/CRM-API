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
        public async Task<Response<TblLoanMaster>> GetLoanDetails(string? filterString, string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblLoanMaster>().AsQueryable();

            if (filterString != null)
            {
                filterData = _context.TblLoanMasters.Where(x => x.IsDeleted != true && x.TblLoanTypeMaster.LoanType.ToLower() == filterString.ToLower()).Include(b => b.TblBankMaster)
                                                               .Include(l => l.TblLoanTypeMaster).Include(u => u.TblUserMaster)
                                                               .ThenInclude(c => c.TblUserCategoryMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblLoanMasters.Where(x => x.IsDeleted != true).Include(b => b.TblBankMaster)
                                                               .Include(l => l.TblLoanTypeMaster).Include(u => u.TblUserMaster)
                                                               .ThenInclude(c => c.TblUserCategoryMaster).AsQueryable();
            }

            if (search != null)
            {
                if (filterString != null)
                {
                    filterData = _context.Search<TblLoanMaster>(search).Where(x => x.IsDeleted != true && x.TblLoanTypeMaster.LoanType.ToLower() == filterString.ToLower()).Include(b => b.TblBankMaster)
                                                           .Include(l => l.TblLoanTypeMaster).Include(u => u.TblUserMaster)
                                                           .ThenInclude(c => c.TblUserCategoryMaster).AsQueryable();
                }
                else
                {
                    filterData = _context.Search<TblLoanMaster>(search).Where(x => x.IsDeleted != true).Include(b => b.TblBankMaster)
                                                               .Include(l => l.TblLoanTypeMaster).Include(u => u.TblUserMaster)
                                                               .ThenInclude(c => c.TblUserCategoryMaster).AsQueryable();
                }
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

        #region Get All Bank Details
        public async Task<Response<TblBankMaster>> GetBankDetails(SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblBankMasters.AsQueryable();

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var bankDetailsResponse = new Response<TblBankMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return bankDetailsResponse;
        }
        #endregion

        #region Get Loan Detail By Id
        public async Task<TblLoanMaster> GetLoanDetailById(int id)
        {
            var loanDetail = await _context.TblLoanMasters.Where(x => x.Id == id && x.IsDeleted != true).Include(u => u.TblUserMaster).Include(c => c.TblLoanTypeMaster)
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
