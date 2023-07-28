using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Account_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CRM_api.DataAccess.Repositories.Account_Module
{
    public class AccountTransactionRepository : IAccountTransactionRepository
    {
        private readonly CRMDbContext _context;

        public AccountTransactionRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Transaction Doc No
        public async Task<TblAccountTransaction> GetLastAccountTrasaction(string? filterString, string? number)
        {
            var transaction = await _context.TblAccountTransactions.Where(x => x.DocType == filterString && x.DocNo.ToLower().Contains(number.ToLower())).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return transaction;
        }
        #endregion

        #region Get Investment Type by Name
        public async Task<TblInvesmentType> GetInvestmentType(string? name)
        {
            var investment = await _context.TblInvesmentTypes.Where(x => x.InvestmentName == name).FirstOrDefaultAsync();
            return investment;
        }
        #endregion

        #region Get Account Transaction
        public async Task<Response<TblAccountTransaction>> GetAccountTransaction(string filterString, string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblAccountTransaction> accountTransaction = new List<TblAccountTransaction>().AsQueryable();

            if (searchingParams != null)
                accountTransaction = _context.Search<TblAccountTransaction>(searchingParams).Where(x => x.DocType == filterString).Include(x => x.DebitAccount).Include(x => x.CreditAccount).Include(x => x.UserMaster).Include(x => x.CompanyMaster).AsQueryable();
            else
                accountTransaction = _context.TblAccountTransactions.Where(x => x.DocType == filterString).Include(x => x.DebitAccount).Include(x => x.CreditAccount).Include(x => x.UserMaster).Include(x => x.CompanyMaster).AsQueryable();

            pageCount = Math.Ceiling(accountTransaction.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(accountTransaction, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var accountTransactionResponse = new Response<TblAccountTransaction>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return accountTransactionResponse;
        }
        #endregion

        #region Get Company And Account wise Account Transaction
        public async Task<List<TblAccountTransaction>> GetCompanyAndAccountWiseTransaction(int? companyId, int accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams)
        {
            var filterData = new List<TblAccountTransaction>().AsQueryable();

            if (search is not null)
                    filterData = _context.Search<TblAccountTransaction>(search).Where(x => (companyId == null || x.Companyid == companyId) && (x.CreditAccountId == accountId || x.DebitAccountId == accountId) && x.DocDate.Value.Date >= startDate.Date && x.DocDate.Value.Date <= endDate.Date).Include(x => x.DebitAccount).Include(x => x.CreditAccount).Include(x => x.UserMaster).Include(x => x.CompanyMaster).AsQueryable();
            else 
                filterData = _context.TblAccountTransactions.Where(x => (companyId == null || x.Companyid == companyId) && (x.CreditAccountId == accountId || x.DebitAccountId == accountId) && x.DocDate.Value.Date >= startDate.Date && x.DocDate.Value.Date <= endDate.Date).Include(x => x.DebitAccount).Include(x => x.CreditAccount).Include(x => x.UserMaster).Include(x => x.CompanyMaster).AsQueryable();

            // Apply Sortintg
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending).ToList();

            return sortedData;
        }
        #endregion

        #region Add Account Transaction
        public async Task<int> AddAccountTransaction(TblAccountTransaction tblAccountTransaction)
        {
            await _context.TblAccountTransactions.AddAsync(tblAccountTransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Account Transaction
        public async Task<int> UpdateAccountTransaction(TblAccountTransaction tblAccountTransaction)
        {
            _context.TblAccountTransactions.Update(tblAccountTransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
