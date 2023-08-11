using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module
{
    public interface IAccountRepository
    {
        Task<(Response<TblAccountMaster>, double?, double?)> GetUserAccount(int? companyId, string? searchingParams, SortingParams sortingParams);
        Task<List<TblAccountMaster>> GetUserAccountById(int? accountId, int? companyId = null);
        Task<Response<TblAccountGroupMaster>> GetAccountGroups(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblAccountGroupMaster>> GetRootAccountGroup(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblCompanyMaster>> GetCompanies(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblFinancialYearMaster>> GetFinancialYears(string? searchingParams, SortingParams sortingParams);
        Task<TblFinancialYearMaster> GetFinancialYearById(int id);
        Task<Response<TblAccountOpeningBalance>> GetAccountOpeningBalance(int? financialYearId, string? searchingParams, SortingParams sortingParams);
        Task<Response<TblAccountMaster>> GetKAGroupBankAndPaymentAccounts(int? companyId, string? filterString, string? search, SortingParams sortingParams);
        Task<int> GetAccountByUserId(int userId);
        Task<int> AddUserAccount(TblAccountMaster tblAccountMaster);
        Task<int> AddAccountGroup(TblAccountGroupMaster tblAccountGroupMaster);
        Task<int> AddCompany(TblCompanyMaster tblCompanyMaster);
        Task<int> AddFinancialYear(TblFinancialYearMaster tblFinancialYear);
        Task<int> AddAccountOpeningBalance(TblAccountOpeningBalance tblAccountOpening);
        Task<int> UpdateUserAccount(TblAccountMaster tblAccountMaster);
        Task<int> UpdateAccountGroup(TblAccountGroupMaster tblAccountGroup);
        Task<int> UpdateCompany(TblCompanyMaster tblCompanyMaster);
        Task<int> UpdateFinancialYear(TblFinancialYearMaster tblFinancialYear);
        Task<int> UpdateAccountOpeningBalance(TblAccountOpeningBalance tblAccountOpening);
        Task<int> DeactivateUserAccount(int id);
        Task<int> DeactivateAccountGroup(int id);
        Task<int> DeactivateCompany(int id);
        Task<int> DeactivateFinancialYear(int id);
        Task<int> DeactivateAccountOpeningBalance(int id);
    }
}
