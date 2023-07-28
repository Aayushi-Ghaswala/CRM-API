using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CRM_api.DataAccess.Repositories.Account_Module
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CRMDbContext _context;

        public AccountRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get User Account
        public async Task<Response<TblAccountMaster>> GetUserAccount(int? companyId, string? searchingParams, SortingParams sortingParams)
        {
            IQueryable<TblAccountMaster> userAccount = new List<TblAccountMaster>().AsQueryable();
            double? pageCount = 0;

            if (searchingParams != null)
                userAccount = _context.Search<TblAccountMaster>(searchingParams).Where(x => (companyId == null || x.Companyid == companyId)).Include(x => x.TblAccountGroupMaster).Include(x => x.TblCompanyMaster).Include(x => x.UserMaster).AsQueryable();
            else
                userAccount = _context.TblAccountMasters.Where(x => (companyId == null || x.Companyid == companyId)).Include(x => x.TblAccountGroupMaster).Include(x => x.TblCompanyMaster).Include(x => x.UserMaster).AsQueryable();

            pageCount = Math.Ceiling(userAccount.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(userAccount, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var userAccountResponse = new Response<TblAccountMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return userAccountResponse;
        }
        #endregion

        #region Get Account Groups
        public async Task<Response<TblAccountGroupMaster>> GetAccountGroups(string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            List<TblAccountGroupMaster> accountGroup = new List<TblAccountGroupMaster>();

            if (searchingParams != null)
                accountGroup = await _context.Search<TblAccountGroupMaster>(searchingParams).Where(x => x.Isdeleted != true).Include(x => x.ParentGroup).Include(x => x.RootGroup).ToListAsync();
            else
                accountGroup = await _context.TblAccountGroupMasters.Where(x => x.Isdeleted != true).Include(x => x.ParentGroup).Include(x => x.RootGroup).ToListAsync();

            var rootGroup = await _context.TblAccountGroupMasters.Where(x => x.AccountGrpName.ToLower().Equals("Root".ToLower())).FirstOrDefaultAsync();
            accountGroup.Remove(rootGroup);

            IQueryable<TblAccountGroupMaster> accountGroups = accountGroup.AsQueryable();

            pageCount = Math.Ceiling(accountGroup.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(accountGroups, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var accountGroupResponse = new Response<TblAccountGroupMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return accountGroupResponse;
        }
        #endregion

        #region Get Root Account Groups
        public async Task<Response<TblAccountGroupMaster>> GetRootAccountGroup(string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblAccountGroupMaster> rootAccountGroups = new List<TblAccountGroupMaster>().AsQueryable();

            var rootId = _context.TblAccountGroupMasters.Where(x => x.AccountGrpName.ToLower().Equals("root")).First().Id;
            rootAccountGroups = _context.TblAccountGroupMasters.Where(x => x.ParentGrpid == rootId).Include(x => x.ParentGroup).Include(x => x.RootGroup).AsQueryable();

            if (searchingParams != null)
                rootAccountGroups = rootAccountGroups.Where(x => x.AccountGrpName.ToLower().Contains(searchingParams.ToLower())).AsQueryable();
            else
                rootAccountGroups = _context.TblAccountGroupMasters.Where(x => x.ParentGrpid == rootId).Include(x => x.ParentGroup).Include(x => x.RootGroup).AsQueryable();

            pageCount = Math.Ceiling(rootAccountGroups.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(rootAccountGroups, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var rootGroupResponse = new Response<TblAccountGroupMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return rootGroupResponse;
        }
        #endregion

        #region Get Company
        public async Task<Response<TblCompanyMaster>> GetCompanies(string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblCompanyMaster> companies = new List<TblCompanyMaster>().AsQueryable();

            if (searchingParams != null)
                companies = _context.Search<TblCompanyMaster>(searchingParams).Where(x => x.Isdeleted != true).AsQueryable();
            else
                companies = _context.TblCompanyMasters.Where(x => x.Isdeleted != true).AsQueryable();

            pageCount = Math.Ceiling(companies.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(companies, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var companyResponse = new Response<TblCompanyMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return companyResponse;
        }
        #endregion

        #region Get Financial Years
        public async Task<Response<TblFinancialYearMaster>> GetFinancialYears(string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblFinancialYearMaster> financialYears = new List<TblFinancialYearMaster>().AsQueryable();

            if (searchingParams != null)
                financialYears = _context.Search<TblFinancialYearMaster>(searchingParams).Where(x => x.Isdeleted != true).AsQueryable();
            else
                financialYears = _context.TblFinancialYearMasters.Where(x => x.Isdeleted != true).AsQueryable();

            pageCount = Math.Ceiling(financialYears.Count() / sortingParams.PageSize);
            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(financialYears, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var financialYearResponse = new Response<TblFinancialYearMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return financialYearResponse;
        }
        #endregion

        #region Get Account Opening Balance
        public async Task<Response<TblAccountOpeningBalance>> GetAccountOpeningBalance(string? searchingParams, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblAccountOpeningBalance> accountOpeningBalance = new List<TblAccountOpeningBalance>().AsQueryable();

            if (searchingParams != null)
                accountOpeningBalance = _context.Search<TblAccountOpeningBalance>(searchingParams).Where(x => x.Isdeleted != true).Include(x => x.TblFinancialYear).Include(x => x.TblAccountMaster).AsQueryable();
            else
                accountOpeningBalance = _context.TblAccountOpeningBalances.Where(x => x.Isdeleted != true).Include(x => x.TblFinancialYear).Include(x => x.TblAccountMaster).AsQueryable();

            pageCount = Math.Ceiling(accountOpeningBalance.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(accountOpeningBalance, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var accountOpeningBalanceResponse = new Response<TblAccountOpeningBalance>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return accountOpeningBalanceResponse;
        }
        #endregion

        #region Get KA Group Account By UserId
        public async Task<Response<TblAccountMaster>> GetKAGroupAccountByUserId(string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblAccountMaster>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblAccountMaster>(search).Where(x => x.UserId == 1014 && x.Isdeleted != true).Include(x => x.TblCompanyMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblAccountMasters.Where(x => x.UserId == 1014 && x.Isdeleted != true).Include(x => x.TblCompanyMaster).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var accountResponse = new Response<TblAccountMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return accountResponse;
        }
        #endregion

        #region Add User Account
        public async Task<int> AddUserAccount(TblAccountMaster tblAccountMaster)
        {
            if (_context.TblAccountMasters.Any(x => x.UserId == tblAccountMaster.UserId && x.AccountName == tblAccountMaster.AccountName && x.Isdeleted != true))
                return 0;

            await _context.TblAccountMasters.AddAsync(tblAccountMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Account Group
        public async Task<int> AddAccountGroup(TblAccountGroupMaster tblAccountGroupMaster)
        {
            if (_context.TblAccountGroupMasters.Any(x => x.AccountGrpName == tblAccountGroupMaster.AccountGrpName && x.Isdeleted != true))
                return 0;

            await _context.TblAccountGroupMasters.AddAsync(tblAccountGroupMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Company
        public async Task<int> AddCompany(TblCompanyMaster tblCompanyMaster)
        {
            if (_context.TblCompanyMasters.Any(x => x.Name == tblCompanyMaster.Name && x.Isdeleted != true))
                return 0;

            await _context.TblCompanyMasters.AddAsync(tblCompanyMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Financial Year
        public async Task<int> AddFinancialYear(TblFinancialYearMaster tblFinancialYear)
        {
            if (_context.TblFinancialYearMasters.Any(x => x.Year == tblFinancialYear.Year && x.Isdeleted != true))
                return 0;

            await _context.TblFinancialYearMasters.AddAsync(tblFinancialYear);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Account Opening Balance
        public async Task<int> AddAccountOpeningBalance(TblAccountOpeningBalance tblAccountOpening)
        {
            if (_context.TblAccountOpeningBalances.Any(x => x.AccountId == tblAccountOpening.AccountId && x.FinancialYearid == tblAccountOpening.FinancialYearid && x.Isdeleted != true))
                return 0;

            await _context.TblAccountOpeningBalances.AddAsync(tblAccountOpening);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Account
        public async Task<int> UpdateUserAccount(TblAccountMaster tblAccountMaster)
        {
            _context.TblAccountMasters.Update(tblAccountMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Account Group
        public async Task<int> UpdateAccountGroup(TblAccountGroupMaster tblAccountGroup)
        {
            _context.TblAccountGroupMasters.Update(tblAccountGroup);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Company
        public async Task<int> UpdateCompany(TblCompanyMaster tblCompanyMaster)
        {
            _context.TblCompanyMasters.Update(tblCompanyMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Financial Year
        public async Task<int> UpdateFinancialYear(TblFinancialYearMaster tblFinancialYear)
        {
            _context.TblFinancialYearMasters.Update(tblFinancialYear);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Account Account Balance
        public async Task<int> UpdateAccountOpeningBalance(TblAccountOpeningBalance tblAccountOpening)
        {
            _context.TblAccountOpeningBalances.Update(tblAccountOpening);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate User Account
        public async Task<int> DeactivateUserAccount(int id)
        {
            var userAccount = await _context.TblAccountMasters.FindAsync(id);

            if (userAccount == null) return 0;

            userAccount.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Account Groups
        public async Task<int> DeactivateAccountGroup(int id)
        {
            var accountGroup = await _context.TblAccountGroupMasters.FindAsync(id);

            if (accountGroup == null) return 0;

            accountGroup.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Company
        public async Task<int> DeactivateCompany(int id)
        {
            var company = await _context.TblCompanyMasters.FindAsync(id);

            if (company == null) return 0;

            company.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Financial Year
        public async Task<int> DeactivateFinancialYear(int id)
        {
            var financialYear = await _context.TblFinancialYearMasters.FindAsync(id);

            if (financialYear == null) return 0;

            financialYear.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Account Opening Balance
        public async Task<int> DeactivateAccountOpeningBalance(int id)
        {
            var accountOpeningBalance = await _context.TblAccountOpeningBalances.FindAsync(id);

            if (accountOpeningBalance == null) return 0;

            accountOpeningBalance.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
