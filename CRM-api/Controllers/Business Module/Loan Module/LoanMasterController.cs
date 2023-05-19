using CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module;
using CRM_api.Services.IServices.Business_Module.Loan_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Loan_Module
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LoanMasterController : Controller
    {
        private readonly ILoanMasterService _loanMasterService;

        public LoanMasterController(ILoanMasterService loanMasterService)
        {
            _loanMasterService = loanMasterService;
        }

        [HttpGet]
        #region Get All Loan Details
        public async Task<IActionResult> GetLoanDetails(int page)
        {
            try
            {
                var loanDetails = await _loanMasterService.GetLoanDetailsAsync(page);
                if (loanDetails.Values.Count == 0)
                    return BadRequest(new { Message = "Loan Detail Not Found"});

                return Ok(loanDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Loan Detail By Id
        public async Task<IActionResult> GetLoanDetailById(int id)
        {
            try
            {
                var loanDetail = await _loanMasterService.GetLoanDetailByIdAsync(id);
                if (loanDetail == null)
                    return BadRequest(new { Message = "Loan Detail Not Found"});

                return Ok(loanDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost]
        #region Add Loan Detail
        public async Task<IActionResult> AddLoanDetail(AddLoanMasterDto loanMasterDto)
        {
            try
            {
                var loan = await _loanMasterService.AddLoanDetailAsync(loanMasterDto);
                return loan !=0 ? Ok(new { Message = "Loan added successfully"}) : BadRequest(new { Message = "Unable to add loan"});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut]
        #region Update Loan Detail
        public async Task<IActionResult> UpdateLoanDetail(UpdateLoanMasterDto loanMasterDto)
        {
            try
            {
                var loan = await _loanMasterService.UpdateLoanDetailAsync(loanMasterDto);
                return loan != 0 ? Ok(new { Message = "Loan updated successfully"}) : BadRequest(new { Message = "Unable to update loan"});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpDelete]
        #region Deactivate Loan Detail
        public async Task<IActionResult> DeactivateLoanDetail(int id)
        {
            var loan = await _loanMasterService.DeactivateLoanDetailAsync(id);
            return loan != 0 ? Ok(new { Message = "Loan deactivated successfully"}) : BadRequest(new { Message = "Unable to deactivate loan"});
        }
        #endregion
    }
}
