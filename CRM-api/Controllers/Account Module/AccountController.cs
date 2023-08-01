using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.IServices.Account_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Account_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #region Get User Account
        [HttpGet("GetUserAccount")]
        public async Task<IActionResult> GetUserAccount(int? companyId, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _accountService.GetUserAccountsAsync(companyId, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Account Groups
        [HttpGet("GetAccountGroups")]
        public async Task<IActionResult> GetAccountGroups([FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _accountService.GetAccountGroupsAsync(search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Root Account Groups
        [HttpGet("GetRootAccountGroups")]
        public async Task<IActionResult> GetRootAccountGroups([FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _accountService.GetRootAccountGroupAsync(search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Companies
        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompanies([FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _accountService.GetCompnanyAsync(search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Financial Year
        [HttpGet("GetFinancialYear")]
        public async Task<IActionResult> GetFinancialYear([FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _accountService.GetFinancialYearAsync(search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Account Opening Balance
        [HttpGet("GetAccountOpeningBalance")]
        public async Task<IActionResult> GetAccountOpeningBalance([FromQuery] string? search, [FromQuery] SortingParams sortingParams, int? financialYearId = null)
        {
            try
            {
                var getData = await _accountService.GetAccountOpeningBalanceAsync(financialYearId, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get KA Group Accounts
        [HttpGet("GetKAGroupAccounts")]
        public async Task<IActionResult> GetKAGroupAccounts(string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var data = await _accountService.GetKAGroupAccountsAsync(search, sortingParams);
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add User Account
        [HttpPost("AddUserAccount")]
        public async Task<IActionResult> AddUserAccount(AddUserAccountDto addUserAccount)
        {
            try
            {
                var data = await _accountService.AddUserAccountAsync(addUserAccount);
                return data != 0 ? Ok(new { Message = "User account added successfully." }) : BadRequest(new { Message = "Unable to add Account" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Account Group
        [HttpPost("AddAccountGroup")]
        public async Task<IActionResult> AddAccountGroup(AddAccountGroupDto addAccountGroup)
        {
            try
            {
                var data = await _accountService.AddAccountGroupAsync(addAccountGroup);
                return data != 0 ? Ok(new { Message = "Account group added successfully." }) : BadRequest(new { Message = "Unable to add account group" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Company
        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany(AddCompanyDto addCompany)
        {
            try
            {
                var data = await _accountService.AddCompanyAsync(addCompany);
                return data != 0 ? Ok(new { Message = "Company added successfully." }) : BadRequest(new { Message = "Unable to add company" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Financial Year
        [HttpPost("AddFinancialYear")]
        public async Task<IActionResult> AddFinancialYear(AddFinancialYearDto addFinancialYear)
        {
            try
            {
                var data = await _accountService.AddFinancialYearAsync(addFinancialYear);
                return data != 0 ? Ok(new { Message = "Financial year added successfully." }) : BadRequest(new { Message = "Unable to add financial year." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Account Opening Balance
        [HttpPost("AddAccountOpeningBalance")]
        public async Task<IActionResult> AddAccountOpeningBalance(AddAccountOpeningBalanceDto addAccountOpeningBalance)
        {
            try
            {
                var data = await _accountService.AddAccountOpeningBalanceAsync(addAccountOpeningBalance);
                return data != 0 ? Ok(new { Message = "Account opening balance added successfully." }) : BadRequest(new { Message = "Unable to add Account opening balance." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update User Account
        [HttpPut("UpdateUserAccount")]
        public async Task<IActionResult> UpdateUserAccount(UpdateUserAccountDto updateUserAccount)
        {
            try
            {
                var data = await _accountService.UpdateUserAccountAsync(updateUserAccount);
                return data != 0 ? Ok(new { Message = "User account updated successfully." }) : BadRequest(new { Message = "Unable to update user account." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Account Group
        [HttpPut("UpdateAccountGroup")]
        public async Task<IActionResult> UpdateAccountGroup(UpdateAccountGroupDto updateAccountGroup)
        {
            try
            {
                var data = await _accountService.UpdateAccountGroupAsync(updateAccountGroup);
                return data != 0 ? Ok(new { Message = "Account Group updated successfully." }) : BadRequest(new { Message = "Unable to update account group." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Company
        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(UpdateCompanyDto updateCompany)
        {
            try
            {
                var data = await _accountService.UpdateCompanyAsync(updateCompany);
                return data != 0 ? Ok(new { Message = "Company updated successfully." }) : BadRequest(new { Message = "Unable to update company." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Financial Year
        [HttpPut("UpdateFinancialYear")]
        public async Task<IActionResult> UpdateFinancialYear(UpdateFinancialYearDto updateFinancialYear)
        {
            try
            {
                var data = await _accountService.UpdateFinancialYearAsync(updateFinancialYear);
                return data != 0 ? Ok(new { Message = "Financial year updated successfully." }) : BadRequest(new { Message = "Unable to update financial year." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Account Opening Balance
        [HttpPut("UpdateAccountOpeningBalance")]
        public async Task<IActionResult> UpdateAccountOpeningBalance(UpdateAccountOpeningBalanceDto updateAccountOpeningBalance)
        {
            try
            {
                var data = await _accountService.UpdatAccountOpeningBalanceAsync(updateAccountOpeningBalance);
                return data != 0 ? Ok(new { Message = "Account opening balance updated successfully." }) : BadRequest(new { Message = "Unable to update account opening balance." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate User Account
        [HttpDelete("DeactivateUserAccount")]
        public async Task<IActionResult> DeactivateUserAccount(int id)
        {
            try
            {
                var userAccount = await _accountService.DeactivateUserAccountAsync(id);
                return userAccount != 0 ? Ok(new { Message = "User account deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate user account." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Account Group
        [HttpDelete("DeactivateAccountGroup")]
        public async Task<IActionResult> DeactivateAccountGroup(int id)
        {
            try
            {
                var accountGroup = await _accountService.DeactivateAccountGroupAsync(id);
                return accountGroup != 0 ? Ok(new { Message = "Account group deactivate successfully." }) : BadRequest(new { Message = "Unable to deactivate account group." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Company
        [HttpDelete("DeactivateCompany")]
        public async Task<IActionResult> DeactivateCompany(int id)
        {
            try
            {
                var company = await _accountService.DeactivateCompanyAsync(id);
                return company != 0 ? Ok(new { Message = "Company deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate company." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Financial Year
        [HttpDelete("DeactivateFinancialYear")]
        public async Task<IActionResult> DeactivateFinancialYear(int id)
        {
            try
            {
                var financialYear = await _accountService.DeactivateFinancialYearAsync(id);
                return financialYear != 0 ? Ok(new { Message = "Financial year deactivate successfully." }) : BadRequest(new { Message = "Unable to deactivate financial year." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Account Opening Balance
        [HttpDelete("DeactivateAccountOpeningBalance")]
        public async Task<IActionResult> DeactivateAccountOpeningBalance(int id)
        {
            try
            {
                var accountOpeningBalance = await _accountService.DeactivateAccountOpeningBalanceAsync(id);
                return accountOpeningBalance != 0 ? Ok(new { Message = "Account opening balance deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate account opening balance." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
