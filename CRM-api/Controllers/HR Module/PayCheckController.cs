using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRM_api.Controllers.HR_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayCheckController : ControllerBase
    {
        private readonly IPayCheckService _payCheckService;

        public PayCheckController(IPayCheckService payCheckService)
        {
            _payCheckService = payCheckService;
        }

        #region Get all PayChecks
        [HttpGet]
        public async Task<IActionResult> GetPayCheck([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var payChecks = await _payCheckService.GetPayCheckAsync(search, sortingParams);

                return Ok(payChecks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get PayCheck By Designation
        [HttpGet("GetPayCheckByDesignation")]
        public async Task<ActionResult<PayCheckDto>> GetPayCheckByDesignation(int designationId)
        {
            try
            {
                var payCheck = await _payCheckService.GetPayCheckByDesignationAsync(designationId);
                return payCheck.PayCheckId != 0 ? Ok(payCheck) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get PayCheck By Id
        [HttpGet("GetPayCheckById")]
        public async Task<ActionResult<PayCheckDto>> GetPayCheckById(int id)
        {
            try
            {
                var payCheck = await _payCheckService.GetPayCheckByIdAsync(id);
                return payCheck.PayCheckId != 0 ? Ok(payCheck) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add PayCheck
        [HttpPost]
        public async Task<ActionResult> AddPayCheck(AddPayCheckDto addPayCheckDto)
        {
            try
            {
                int row = await _payCheckService.AddPayCheckAsync(addPayCheckDto);
                return row > 0 ? Ok(new { Message = "PayCheck added successfully."}) : BadRequest(new { Message = "Unable to add payCheck."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update PayCheck
        [HttpPut]
        public async Task<ActionResult> UpdatePayCheck(UpdatePayCheckDto updatePayCheckDto)
        {
            try
            {
                int row = await _payCheckService.UpdatePayCheckAsync(updatePayCheckDto);
                return row != 0 ? Ok(new { Message = "PayCheck updated successfully."}) : BadRequest(new { Message = "Unable to update payCheck."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate payCheck
        [HttpDelete]
        public async Task<IActionResult> DeactivatePayCheck(int id)
        {
            var payCheck = _payCheckService.DeactivatePayCheckAsync(id);
            return await payCheck !=0 ? Ok(new { Message = "PayCheck deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate payCheck."});
        }
        #endregion
    }
}
