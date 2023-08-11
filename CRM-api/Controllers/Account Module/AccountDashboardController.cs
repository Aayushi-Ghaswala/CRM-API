using CRM_api.DataAccess.Helper;
using CRM_api.Services.IServices.Account_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Account_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDashboardController : ControllerBase
    {
        private readonly IAccountTransactionservice _accountTransactionService;

        public AccountDashboardController(IAccountTransactionservice accountTransactionService)
        {
            _accountTransactionService = accountTransactionService;
        }

        #region Get Company And Account wise Account Transaction
        [HttpGet("GetCompanyAndAccountWiseTransaction")]
        public async Task<IActionResult> GetCompanyAndAccountWiseTransaction(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, [FromQuery] SortingParams sortingParams, bool isBankBook = false)
        {
            try
            {
                var transactions = await _accountTransactionService.GetCompanyAndAccountWiseTransactionAsync(companyId, accountId, startDate, endDate, search, sortingParams, isBankBook);
                var values = transactions.Item1;
                return Ok(new { values, Total = transactions.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Company Wise Trial Balance
        [HttpGet("GetCompanyWiseTrailBalance")]
        public async Task<IActionResult> GetCompanyWiseTrailBalance(int companyId, DateTime startDate, DateTime endDate, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var trialBalance = await _accountTransactionService.CalculateTrailBalanceByCompanyIdAsync(companyId, startDate, endDate, search, sortingParams);
                return Ok(new { Values = trialBalance.Item1, Total = trialBalance.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Company Wise Journal Entries
        [HttpGet("GetCompanyWiseJVTransaction")]
        public async Task<IActionResult> GetCompanyWiseJVTransaction(int companyId, DateTime startDate, DateTime endDate, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var journalTransactions = await _accountTransactionService.GetCompanyWiseJVTransactionAsync(companyId, startDate, endDate, search, sortingParams);
                var values = journalTransactions.Item1;
                return Ok(new { values, Total = journalTransactions.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
