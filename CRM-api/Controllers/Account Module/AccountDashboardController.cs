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
        public async Task<IActionResult> GetCompanyAndAccountWiseTransaction(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var data = await _accountTransactionService.GetCompanyAndAccountWiseTransactionAsync(companyId, accountId, startDate, endDate, search, sortingParams);
                return Ok(new {Values = data.Item1, Total = data.Item2});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
