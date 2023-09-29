using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Account_Module
{
    public interface IAccountService
    {
        Task<(ResponseDto<AccountMasterDto>, Dictionary<string, double?>)> GetUserAccountsAsync(int? userId, int? companyId, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<AccountGroupDto>> GetAccountGroupsAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<AccountGroupDto>> GetRootAccountGroupAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<CompanyMasterDto>> GetCompnanyAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<FinancialYearDto>> GetFinancialYearAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<AccountOpeningBalanceDto>> GetAccountOpeningBalanceAsync(int? financialYearId, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<AccountMasterDto>> GetKAGroupBankAndPaymentAccountsAsync(int? companyId, string? filterString, string? search, SortingParams sortingParams);
        Task<int> AddUserAccountAsync(AddUserAccountDto addUserAccount);
        Task<int> AddAccountGroupAsync(AddAccountGroupDto addAccountGroup);
        Task<int> AddCompanyAsync(AddCompanyDto addCompany);
        Task<int> AddFinancialYearAsync(AddFinancialYearDto addFinancialYear);
        Task<int> AddAccountOpeningBalanceAsync(AddAccountOpeningBalanceDto addAccountOpeningBalance);
        Task<int> UpdateUserAccountAsync(UpdateUserAccountDto updateUserAccount);
        Task<int> UpdateAccountGroupAsync(UpdateAccountGroupDto updateAccountGroup);
        Task<int> UpdateCompanyAsync(UpdateCompanyDto updateCompany);
        Task<int> UpdateFinancialYearAsync(UpdateFinancialYearDto updateFinancialYear);
        Task<int> UpdatAccountOpeningBalanceAsync(UpdateAccountOpeningBalanceDto updateAccountOpeningBalance);
        Task<int> DeactivateUserAccountAsync(int id);
        Task<int> DeactivateAccountGroupAsync(int id);
        Task<int> DeactivateCompanyAsync(int id);
        Task<int> DeactivateFinancialYearAsync(int id);
        Task<int> DeactivateAccountOpeningBalanceAsync(int id);
    }
}
