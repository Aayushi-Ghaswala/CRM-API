using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module;
using CRM_api.Services.IServices.Business_Module.Loan_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Loan_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanMasterController : Controller
    {
        private readonly ILoanMasterService _loanMasterService;

        public LoanMasterController(ILoanMasterService loanMasterService)
        {
            _loanMasterService = loanMasterService;
        }

        #region Get All Loan Details
        [HttpGet("GetLoanDetails")]
        public async Task<IActionResult> GetLoanDetails(string? filterString, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var loanDetails = await _loanMasterService.GetLoanDetailsAsync(filterString, search, sortingParams);
                return Ok(loanDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get All Loan Detail By Id
        [HttpGet("GetLoanDetailById")]
        public async Task<IActionResult> GetLoanDetailById(int id)
        {
            try
            {
                var loanDetail = await _loanMasterService.GetLoanDetailByIdAsync(id);
                if (loanDetail == null)
                    return BadRequest(new { Message = "Loan detail not found." });

                return Ok(loanDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get All Bank Details
        [HttpGet("GetBankDetails")]
        public async Task<IActionResult> GetBankDetails([FromQuery] SortingParams sortingParams)
        {
            try
            {
                var bankDetails = await _loanMasterService.GetBankDetailsAsync(sortingParams);

                return Ok(bankDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Loan Detail
        [HttpPost("AddLoanDetail")]
        public async Task<IActionResult> AddLoanDetail(AddLoanMasterDto loanMasterDto)
        {
            try
            {
                var loan = await _loanMasterService.AddLoanDetailAsync(loanMasterDto);
                return loan != 0 ? Ok(new { Message = "Loan added successfully." }) : BadRequest(new { Message = "Unable to add loan." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Loan Detail
        [HttpPut("UpdateLoanDetail")]
        public async Task<IActionResult> UpdateLoanDetail(UpdateLoanMasterDto loanMasterDto)
        {
            try
            {
                var loan = await _loanMasterService.UpdateLoanDetailAsync(loanMasterDto);
                return loan != 0 ? Ok(new { Message = "Loan updated successfully." }) : BadRequest(new { Message = "Unable to update loan." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Loan Detail
        [HttpDelete("DeactivateLoanDetail")]
        public async Task<IActionResult> DeactivateLoanDetail(int id)
        {
            var loan = await _loanMasterService.DeactivateLoanDetailAsync(id);
            return loan != 0 ? Ok(new { Message = "Loan deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate loan." });
        }
        #endregion
    }
}
