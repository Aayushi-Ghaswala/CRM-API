﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.MGain_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MGainController : ControllerBase
    {
        private readonly IMGainService _mGainService;

        public MGainController(IMGainService mGainService)
        {
            _mGainService = mGainService;
        }

        [HttpGet("GetMGainDetails")]
        #region Get All MGain Details
        public async Task<IActionResult> GetMGainDetails(int? currancyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var mGainDetails = await _mGainService.GetAllMGainDetailsAsync(currancyId, type, isClosed, fromDate, toDate, search, sortingParams);
                return Ok(mGainDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet("GetPaymentsByMGainId")]
        #region Get Payment Details By MGain Id
        public async Task<IActionResult> GetPaymentByMGainId(int mGainId)
        {
            try
            {
                var getData = await _mGainService.GetPaymentByMgainIdAsync(mGainId);
                return Ok(getData);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet("MGainAggrementHTML")]
        #region MGain Aggrement
        public async Task<IActionResult> MGainAggrement(int Id)
        {
            var MGain = await _mGainService.MGainAggrementAsync(Id);
            return Ok(MGain);
        }
        #endregion

        [HttpGet("MGainPaymentReciept")]
        #region MGain Payment Reciept
        public async Task<IActionResult> MGainPaymentReciept(int id)
        {
            try
            {
                var file = await _mGainService.MGainPaymentReceipt(id);
                return File(file.file, "application/pdf", file.FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 

        [HttpGet("GetAllProjects")]
        #region Get All Projects
        public async Task<IActionResult> GetAllProjects([FromQuery] string? searchingParams, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _mGainService.GetAllProjectAsync(searchingParams, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("GetPlotsByProjectId")]
        #region Get Plots By ProjectId
        public async Task<IActionResult> GetPlotsByProjectId(int projectId, decimal invAmount, [FromQuery] string? searchingParama, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _mGainService.GetPlotsByProjectIdAsync(projectId, invAmount, searchingParama, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("GetAllCurrencies")]
        #region Get All Currencies
        public async Task<IActionResult> GetAllCurrencies()
        {
            try
            {
                var getData = await _mGainService.GetAllCurrenciesAsync();
                return Ok(getData);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost("AddMGainDetails")]
        #region Add MGain Details
        public async Task<IActionResult> AddMGainDetails([FromForm] AddMGainDetailsDto MGainDetailsDto)
        {
            try
            {
                var mGain = await _mGainService.AddMGainDetailAsync(MGainDetailsDto);
                return mGain != null ? Ok(new { Message = "MGain details added successfully.", MGainId = mGain.Id }) : BadRequest(new { Message = "Unable to add mgain details." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost("AddMGainPayment")]
        #region Add MGain Payment Details
        public async Task<IActionResult> AddMGainDetails(List<AddMGainPaymentDto> paymentDtos)
        {
            try
            {
                var mGainPayments = await _mGainService.AddPaymentDetailsAsync(paymentDtos);
                return mGainPayments != 0 ? Ok(new { Message = "Payments details added successfully." }) : BadRequest(new { Message = "Unable to add Payment Details." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPost("MGainAggrementPDF")]
        #region MGain Aggrement PDF
        public async Task<IActionResult> MGainAggrementPDF(int id, [FromForm] string htmlContent)
        {
            try
            {
                var pdf = await _mGainService.GenerateMGainAggrementAsync(id, htmlContent);
                return File(pdf.file, "application/pdf", pdf.FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut("UpdateMGainDetails")]
        #region Update MGain Details
        public async Task<IActionResult> UpdateMGainDetails([FromForm] UpdateMGainDetailsDto updateMGainDetails)
        {
            try
            {
                var updateMGain = await _mGainService.UpdateMGainDetailsAsync(updateMGainDetails);
                return updateMGain != 0 ? Ok(new { Message = "MGain detail updated successfully." }) : BadRequest(new { Message = "Unable to update mgain details" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpPut("UpdateMGainPayment")]
        #region Update MGain Payment Details
        public async Task<IActionResult> UpdateMGainPayment(List<UpdateMGainPaymentDto> updateMGainPayment)
        {
            try
            {
                var updateData = await _mGainService.UpdateMGainPaymentAsync(updateMGainPayment);
                return updateData != 0 ? Ok(new { Message = "MGain payment updated successfully." }) : BadRequest(new { Message = "Unable to update mgain details" });
            }
            catch
            {
                throw;
            }
        }
        #endregion

        [HttpDelete("DeleteMGainPayment")]
        #region Delete MGain Payment Details
        public async Task<IActionResult> DeleteMGainPayment(int id)
        {
            try
            {
                var deleteData = await _mGainService.DeleteMGainPaymentAsync(id);
                return deleteData != 0 ? Ok(new { Message = "MGain payment deleted successfully." }) : BadRequest(new { Message = "Unable to delete mgain payment." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}