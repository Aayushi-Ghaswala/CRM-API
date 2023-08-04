using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.IServices.Account_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Account_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        private readonly IAccountTransactionservice _accountTransactionservice;

        public AccountTransactionController(IAccountTransactionservice accountTransactionservice)
        {
            _accountTransactionservice = accountTransactionservice;
        }

        #region Get Transaction Doc No
        [HttpGet("GetTransactionDocNo")]
        public async Task<IActionResult> GetTransactionDocNo(string? filterString)
        {
            try
            {
                var docNo = await _accountTransactionservice.GetTransactionDocNoAsync(filterString);
                return Ok(new { Message = docNo });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Payment type
        [HttpGet("GetPaymentTypes")]
        public async Task<IActionResult> GetPaymentTypes(string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                return Ok(await _accountTransactionservice.GetPaymentTypesAsync(search, sortingParams));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Account Transaction
        [HttpGet("GetAccountTransaction")]
        public async Task<IActionResult> GetAccountTransaction(int? companyId, int? financialYearId, string? search, [FromQuery] SortingParams sortingParams, [FromQuery] string filterString = null)
        {
            try
            {
                var getData = await _accountTransactionservice.GetAccountTransactionAsync(companyId, financialYearId, filterString, search, sortingParams);
                return Ok(new { Data = getData.Item1, Total = getData.Item2});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Account Transaction
        [HttpPost("AddAccountTransaction")]
        public async Task<IActionResult> AddAccountTransaction(AddAccountTransactionDto addAccountTransaction)
        {
            try
            {
                var accountTransaction = await _accountTransactionservice.AddAccountTransactionAsync(addAccountTransaction);
                return accountTransaction != 0 ? Ok(new { Message = "Account transaction added successfully." }) : BadRequest(new { Message = "Unable to add account transaction." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Account Transaction
        [HttpPut("UpdateAccountTransaction")]
        public async Task<IActionResult> UpdateAccountTransaction(UpdateAccountTransactionDto updateAccountTransaction)
        {
            try
            {
                var accountTransaction = await _accountTransactionservice.UpdateAccountTransactionAsync(updateAccountTransaction);
                return accountTransaction != 0 ? Ok(new { Message = "Account transaction updated successfully." }) : BadRequest(new { Message = "Unable to update account transaction." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Account Transactions
        [HttpDelete("DeleteAccountTransaction")]
        public async Task<IActionResult> DeleteAccountTransaction(string? docNo)
        {
            try
            {
                var data = await _accountTransactionservice.DeleteAccountTransactionAsync(docNo);
                return data != 0 ? Ok(new { Message = "Account transaction deleted successfully." }) : BadRequest(new { Message = "Unable to delete account transaction." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
