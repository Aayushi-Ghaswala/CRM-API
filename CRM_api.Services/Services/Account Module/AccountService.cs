using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Account_Module;
using DocumentFormat.OpenXml.Bibliography;

namespace CRM_api.Services.Services.Account_Module
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        #region Get User Accounts
        public async Task<ResponseDto<AccountMasterDto>> GetUserAccountsAsync(int? companyId, string? searchingParams, SortingParams sortingParams)
        {
            var userAccount = await _accountRepository.GetUserAccount(companyId, searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<AccountMasterDto>>(userAccount);
        }
        #endregion

        #region Get Account Groups
        public async Task<ResponseDto<AccountGroupDto>> GetAccountGroupsAsync(string? searchingParams, SortingParams sortingParams)
        {
            var accountGroups = await _accountRepository.GetAccountGroups(searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<AccountGroupDto>>(accountGroups);
        }
        #endregion

        #region Get Root Account Groups
        public async Task<ResponseDto<AccountGroupDto>> GetRootAccountGroupAsync(string? searchingParams, SortingParams sortingParams)
        {
            var rootAccountGroup = await _accountRepository.GetRootAccountGroup(searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<AccountGroupDto>>(rootAccountGroup);
        }
        #endregion

        #region Get Companies
        public async Task<ResponseDto<CompanyMasterDto>> GetCompnanyAsync(string? searchingParams, SortingParams sortingParams)
        {
            var comapany = await _accountRepository.GetCompanies(searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<CompanyMasterDto>>(comapany);
        }
        #endregion

        #region Get Financial Year
        public async Task<ResponseDto<FinancialYearDto>> GetFinancialYearAsync(string? searchingParams, SortingParams sortingParams)
        {
            var financialYear = await _accountRepository.GetFinancialYears(searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<FinancialYearDto>>(financialYear);
        }
        #endregion

        #region Get Account Opening Balance
        public async Task<ResponseDto<AccountOpeningBalanceDto>> GetAccountOpeningBalanceAsync(int? financialYearId, string? searchingParams, SortingParams sortingParams)
        {
            var accountOpeningBalance = await _accountRepository.GetAccountOpeningBalance(financialYearId, searchingParams, sortingParams);
            return _mapper.Map<ResponseDto<AccountOpeningBalanceDto>>(accountOpeningBalance);
        }
        #endregion

        #region Get KA Group Bank Accounts
        public async Task<ResponseDto<AccountMasterDto>> GetKAGroupAccountsAsync(string? search, SortingParams sortingParams)
        {
            var accounts = await _accountRepository.GetKAGroupBankAccounts(search, sortingParams);
            var mappedAccounts = _mapper.Map<ResponseDto<AccountMasterDto>>(accounts);
            return mappedAccounts;
        }
        #endregion

        #region Add User Account
        public async Task<int> AddUserAccountAsync(AddUserAccountDto addUserAccount)
        {
            var mapUserAccount = _mapper.Map<TblAccountMaster>(addUserAccount);
            mapUserAccount.Isdeleted = false;

            if (DateTime.Now.Month >= 4)
            {
                mapUserAccount.OpeningBalanceDate = Convert.ToDateTime("01-04-" + DateTime.Now.Year);
            }
            else
            {
                mapUserAccount.OpeningBalanceDate = Convert.ToDateTime("01-04-" + (DateTime.Now.Year - 1));
            }

            return await _accountRepository.AddUserAccount(mapUserAccount);
        }
        #endregion

        #region Add Account Group
        public async Task<int> AddAccountGroupAsync(AddAccountGroupDto addAccountGroup)
        {
            var mapAccountGroup = _mapper.Map<TblAccountGroupMaster>(addAccountGroup);
            mapAccountGroup.Isdeleted = false;
            return await _accountRepository.AddAccountGroup(mapAccountGroup);
        }
        #endregion

        #region Add Company
        public async Task<int> AddCompanyAsync(AddCompanyDto addCompany)
        {
            var mapCompany = _mapper.Map<TblCompanyMaster>(addCompany);
            mapCompany.Isdeleted = false;
            return await _accountRepository.AddCompany(mapCompany);
        }
        #endregion

        #region Add Financial Year
        public async Task<int> AddFinancialYearAsync(AddFinancialYearDto addFinancialYear)
        {
            var mapYear = _mapper.Map<TblFinancialYearMaster>(addFinancialYear);
            mapYear.Isdeleted = false;
            return await _accountRepository.AddFinancialYear(mapYear);
        }
        #endregion

        #region Add Account Opening Balance
        public async Task<int> AddAccountOpeningBalanceAsync(AddAccountOpeningBalanceDto addAccountOpeningBalance)
        {
            var mapAccountOpeningBalance = _mapper.Map<TblAccountOpeningBalance>(addAccountOpeningBalance);
            mapAccountOpeningBalance.Isdeleted = false;

            return await _accountRepository.AddAccountOpeningBalance(mapAccountOpeningBalance);
        }
        #endregion

        #region Update User Account
        public async Task<int> UpdateUserAccountAsync(UpdateUserAccountDto updateUserAccount)
        {
            var mapUserAccount = _mapper.Map<TblAccountMaster>(updateUserAccount);
            mapUserAccount.Isdeleted = false;
            return await _accountRepository.UpdateUserAccount(mapUserAccount);
        }
        #endregion

        #region Update Account Group
        public async Task<int> UpdateAccountGroupAsync(UpdateAccountGroupDto updateAccountGroup)
        {
            var mapAccountGroup = _mapper.Map<TblAccountGroupMaster>(updateAccountGroup);
            mapAccountGroup.Isdeleted = false;
            return await _accountRepository.UpdateAccountGroup(mapAccountGroup);
        }
        #endregion

        #region Update Company
        public async Task<int> UpdateCompanyAsync(UpdateCompanyDto updateCompany)
        {
            var mapCompany = _mapper.Map<TblCompanyMaster>(updateCompany);
            mapCompany.Isdeleted = false;
            return await _accountRepository.UpdateCompany(mapCompany);
        }
        #endregion

        #region Update Financial Year
        public async Task<int> UpdateFinancialYearAsync(UpdateFinancialYearDto updateFinancialYear)
        {
            var mapYear = _mapper.Map<TblFinancialYearMaster>(updateFinancialYear);
            mapYear.Isdeleted = false;
            return await _accountRepository.UpdateFinancialYear(mapYear);
        }
        #endregion

        #region Update Account Opening Balance
        public async Task<int> UpdatAccountOpeningBalanceAsync(UpdateAccountOpeningBalanceDto updateAccountOpeningBalance)
        {
            var mapAccountOpeningBalance = _mapper.Map<TblAccountOpeningBalance>(updateAccountOpeningBalance);
            mapAccountOpeningBalance.Isdeleted = false;
            return await _accountRepository.UpdateAccountOpeningBalance(mapAccountOpeningBalance);
        }
        #endregion

        #region Deactivate User Account
        public async Task<int> DeactivateUserAccountAsync(int id)
        {
            return await _accountRepository.DeactivateUserAccount(id);
        }
        #endregion

        #region Deactivate Account Group
        public async Task<int> DeactivateAccountGroupAsync(int id)
        {
            return await _accountRepository.DeactivateAccountGroup(id);
        }
        #endregion

        #region Deactivate Company
        public async Task<int> DeactivateCompanyAsync(int id)
        {
            return await _accountRepository.DeactivateCompany(id);
        }
        #endregion

        #region Deactivate Financial Year
        public async Task<int> DeactivateFinancialYearAsync(int id)
        {
            return await _accountRepository.DeactivateFinancialYear(id);
        }
        #endregion

        #region Deactivate Account Opening Balance
        public async Task<int> DeactivateAccountOpeningBalanceAsync(int id)
        {
            return await _accountRepository.DeactivateAccountOpeningBalance(id);
        }
        #endregion
    }
}
