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

        [HttpPost]
        #region Add Loan Detail
        public async Task<IActionResult> AddLoanDetail(AddLoanMasterDto loanMasterDto)
        {
            try
            {
                await _loanMasterService.AddLoanDetailAsync(loanMasterDto);
                return Ok("Added Successfully...");
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
                await _loanMasterService.UpdateLoanDetailAsync(loanMasterDto);
                return Ok("Updated Successfully...");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet("GetLoanDetails")]
        #region Get All Loan Details
        public async Task<IActionResult> GetLoanDetails(int page)
        {
            try
            {
                var loanDetails = await _loanMasterService.GetLoanDetailsAsync(page);
                if (loanDetails.Values.Count == 0)
                    return BadRequest("Loan Detail Not Found...");

                return Ok(loanDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet("GetLoanDetailById")]
        #region Get All Loan Detail By Id
        public async Task<IActionResult> GetLoanDetailById(int id)
        {
            try
            {
                var loanDetail = await _loanMasterService.GetLoanDetailByIdAsync(id);
                if (loanDetail == null)
                    return BadRequest("Loan Detail Not Found...");

                return Ok(loanDetail);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
