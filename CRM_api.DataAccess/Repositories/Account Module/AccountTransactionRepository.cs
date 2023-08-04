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

        #region Get Payment type
        public async Task<Response<TblPaymentTypeMaster>> GetPaymentType(string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblPaymentTypeMaster>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblPaymentTypeMaster>(search).Where(x => x.IsActive == true).AsQueryable();
            }
            else
            {
                filterData = _context.TblPaymentTypeMasters.Where(x => x.IsActive == true).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var paymentTypeResponse = new Response<TblPaymentTypeMaster>
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

           return paymentTypeResponse;
        }
        #endregion

        #region Get Payment Type By Name
        public async Task<TblPaymentTypeMaster> GetPaymentTypebyName(string? name)
        {
            var paymentType = await _context.TblPaymentTypeMasters.Where(x => x.PaymentName.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            return paymentType;
        }
        #endregion

        #region Get Account Transaction
        public async Task<(Response<TblAccountTransaction>, decimal?, decimal?)> GetAccountTransaction(int? companyId, int? financialYearId, string filterString, string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblAccountTransaction> accountTransaction = new List<TblAccountTransaction>().AsQueryable();

            var financialYear = await _context.TblFinancialYearMasters.FirstOrDefaultAsync(x => x.Id == financialYearId);

            if (searchingParams != null)
                accountTransaction = _context.Search<TblAccountTransaction>(searchingParams).Where(x => x.DocType == filterString && x.DocDate >= financialYear.Startdate && x.DocDate <= financialYear.Enddate && (companyId == null || x.Companyid == companyId)).Include(x => x.TblAccountMaster).Include(x => x.UserMaster).Include(x => x.CompanyMaster).Include(x => x.TblMgainCurrancyMaster).AsQueryable();
            else
                accountTransaction = _context.TblAccountTransactions.Where(x => x.DocType == filterString && x.DocDate >= financialYear.Startdate && x.DocDate <= financialYear.Enddate && (companyId == null || x.Companyid == companyId)).Include(x => x.TblAccountMaster).Include(x => x.UserMaster).Include(x => x.CompanyMaster).Include(x => x.TblMgainCurrancyMaster).AsQueryable(); 

            pageCount = Math.Ceiling(accountTransaction.Count() / sortingParams.PageSize);

            var totalDebit = accountTransaction.Sum(x => x.Debit);
            var totalCredit = accountTransaction.Sum(x => x.Credit);

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

            return (accountTransactionResponse, totalDebit, totalCredit);
        }
        #endregion

        #region Get Company And Account wise Account Transaction
        public async Task<List<TblAccountTransaction>> GetCompanyAndAccountWiseTransaction(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams)
        {
            var filterData = new List<TblAccountTransaction>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblAccountTransaction>(search).Where(x => (companyId == null || x.Companyid == companyId) && x.Accountid == accountId && x.DocDate.Value.Date >= startDate.Date && x.DocDate.Value.Date <= endDate.Date).Include(x => x.TblAccountMaster).Include(x => x.UserMaster).Include(x => x.CompanyMaster).Include(x => x.TblMgainCurrancyMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblAccountTransactions.Where(x => (companyId == null || x.Companyid == companyId) && x.Accountid == accountId && x.DocDate.Value.Date >= startDate.Date && x.DocDate.Value.Date <= endDate.Date).Include(x => x.TblAccountMaster).Include(x => x.UserMaster).Include(x => x.CompanyMaster).Include(x => x.TblMgainCurrancyMaster).AsQueryable();
            }

            // Apply Sortintg
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending).ToList();

            return sortedData;
        }
        #endregion

        #region Get Account Transaction By DocNo
        public async Task<List<TblAccountTransaction>> GetAccountTransactionByDocNo(string docNo, decimal? debit, decimal? credit)
        {
            List<TblAccountTransaction> transactions = new List<TblAccountTransaction>();

            if (debit is not null || credit is not null)
            {
                var transaction = await _context.TblAccountTransactions.Where(x => x.DocNo.Equals(docNo) && x.Debit != debit && x.Credit != credit)
                                                                       .Include(x => x.TblAccountMaster).AsNoTracking().FirstOrDefaultAsync();
                transactions.Add(transaction);
            }
            else
            {
                transactions = await _context.TblAccountTransactions.Where(x => x.DocNo.Equals(docNo)).ToListAsync();
            }

            if (transactions is null)
                return null;

            return transactions;
        }
        #endregion

        #region Ge Account Transaction By Id
        public async Task<TblAccountTransaction> GetAccountTransactionById(int id)
        {
            var accountTransaction = await _context.TblAccountTransactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (accountTransaction is null) return null;

            return accountTransaction;
        }
        #endregion

        #region Add Account Transaction
        public async Task<int> AddAccountTransaction(List<TblAccountTransaction> tblAccountTransactions)
        {
            await _context.TblAccountTransactions.AddRangeAsync(tblAccountTransactions);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Account Transaction
        public async Task<int> UpdateAccountTransaction(List<TblAccountTransaction> tblAccountTransactions)
        {
            _context.TblAccountTransactions.UpdateRange(tblAccountTransactions);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Account Transaction
        public async Task<int> DeleteAccountTransaction(List<TblAccountTransaction> accountTransactions)
        {
            _context.TblAccountTransactions.RemoveRange(accountTransactions);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
