using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Helper.File_Helper;
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

        #region Get All MGain Details
        [HttpGet("GetMGainDetails")]
        public async Task<IActionResult> GetMGainDetails(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var mGainDetails = await _mGainService.GetAllMGainDetailsAsync(currencyId, type, isClosed, fromDate, toDate, search, sortingParams);
                return Ok(mGainDetails);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Payment Details By MGain Id
        [HttpGet("GetPaymentsByMGainId")]
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

        #region MGain Aggrement
        [HttpGet("MGainAggrementHTML")]
        public async Task<IActionResult> MGainAggrement(int Id)
        {
            var MGain = await _mGainService.MGainAggrementAsync(Id);
            return Ok(new { Aggrement = MGain });
        }
        #endregion

        #region MGain Payment Reciept
        [HttpGet("MGainPaymentReciept")]
        public async Task<IActionResult> MGainPaymentReciept(int id)
        {
            try
            {
                var file = await _mGainService.MGainPaymentReceipt(id);
                return file is not null ? Ok(new { Message = File(file.file, "application/pdf", file.FileName) }) : BadRequest(new { Message = "Unable to download payment reciept." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 

        #region Get Base 64 File From Location
        [HttpGet("GetBase64File")]
        public async Task<IActionResult> GetBase64File(string? path)
        {
            try
            {
                var base64 = GetBase64FileHelper.GetBase64File(path);
                var ext = Path.GetExtension(path);
                return Ok(new { Base64 = base64, Extension = ext });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
              
        #region MGain Monthly Non-Cumulative Interest Computation & Release
        [HttpGet("MGainMonthlyNon-CumulativeInterest")]
        public async Task<IActionResult> GetNonCumulativeMonthlyReport(int month, int year, int? schemaId, decimal? tds, bool? isPayment, DateTime? crEntryDate, string? crNarration, bool? isSendSMS, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _mGainService.GetNonCumulativeMonthlyReportAsync(month, year, schemaId, tds, isPayment, crEntryDate, crNarration, search, sortingParams, isSendSMS);

                return getData.Item2 == null ? Ok(getData.Item1) : getData.Item1 == null ? BadRequest(new { Message = getData.Item2 }) : getData.Item1 != null && getData.Item2 != null ? Ok(new { Message = getData.Item2 }) : Ok(new { Message = getData.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Valuation Report By UserId
        [HttpGet("GetValuationReportByUserId")]
        public async Task<IActionResult> GetValuationReportByUserId(int UserId)
        {
            try
            {
                var getData = await _mGainService.GetValuationReportByUserIdAsync(UserId);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Valuation Report PDF
        [HttpPost("ValuationReportPDF")]
        public async Task<IActionResult> ValuationReportPDF(List<MGainValuationDto> mGainValuations)
        {
            try
            {
                var file = await _mGainService.ValuationReportPDF(mGainValuations);
                return file is not null ? Ok(new { Message = File(file.file, "application/pdf", file.FileName) }) : BadRequest(new { Message = "Unable to download valuation report." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MGain Month wise Total Interest Paid
        [HttpGet("GetMonthWiseInterestPaid")]
        public async Task<IActionResult> GetMonthWiseInterestPaid(int month, int year, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            var intrestPaid = await _mGainService.GetMonthWiseInterestPaidAsync(month, year, search, sortingParams);
            return Ok(intrestPaid);
        }
        #endregion

        #region Get MGain Cumulative Interest Computation
        [HttpGet("GetMgGainCumulativeInterestReport")]
        public async Task<IActionResult> GetMgGainCumulativeInterestReport(int fromYear, int toYear, int? schemeId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var details = await _mGainService.GetMgGainCumulativeInterestReportAsync(fromYear, toYear, schemeId, search, sortingParams);
                return Ok(details);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get MGain 10 Years Interest Details
        [HttpGet("GetMGain10YearsInterestDetails")]
        public async Task<IActionResult> GetMGain10YearsInterestDetails(string userName, int schemeId, DateTime invDate, decimal mGainAmount, string mGainType)
        {
            try
            {
                return Ok(await _mGainService.GetMGain10YearsInterestDetailsAsync(userName, schemeId, invDate, mGainAmount, mGainType));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get MGain Interest Certificate
        [HttpGet("GetMGainInterestCertificate")]
        public async Task<ActionResult> GetMGainInterestCertificate(int userId, int financialYearId)
        {
            try
            {
                var file = await _mGainService.GetMGainIntertestCertificateAsync(userId, financialYearId);
                return file != null ? Ok(file) : BadRequest(new { Message = "Interest details not found." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get MGain Interest Ledger
        [HttpGet("GetMGainInterestLedger")]
        public async Task<ActionResult> GetMGainInterestLedger(int userId, int financialYearId)
        {
            try
            {
                var file = await _mGainService.GetMGainInterestLedgerAsync(userId, financialYearId);
                return Ok(file);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Plots By ProjectId
        [HttpGet("GetPlotsByProjectId")]
        public async Task<IActionResult> GetPlotsByProjectId(int projectId, int? plotId)
        {
            try
            {
                var getData = await _mGainService.GetPlotsByProjectIdAsync(projectId, plotId);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Currencies
        [HttpGet("GetAllCurrencies")]
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

        #region Add MGain Details
        [HttpPost("AddMGainDetails")]
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

        #region Add MGain Payment Details
        [HttpPost("AddMGainPayment")]
        public async Task<IActionResult> AddMGainPayment(List<AddMGainPaymentDto> paymentDtos)
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

        #region MGain Aggrement PDF
        [HttpPost("MGainAggrementPDF")]
        public async Task<IActionResult> MGainAggrementPDF(int id, [FromForm] string htmlContent)
        {
            try
            {
                var pdf = await _mGainService.GenerateMGainAggrementAsync(id, htmlContent);
                return Ok(new { file = File(pdf.file, "application/pdf", pdf.FileName) });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update MGain Details
        [HttpPut("UpdateMGainDetails")]
        public async Task<IActionResult> UpdateMGainDetails([FromForm] UpdateMGainDetailsDto updateMGainDetails)
        {
            try
            {
                var updateMGain = await _mGainService.UpdateMGainDetailsAsync(updateMGainDetails);
                return updateMGain.Item1 != 0 ? Ok(new { Message = updateMGain.Item2 }) : BadRequest(new { Message = updateMGain.Item2 });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update MGain Payment Details
        [HttpPut("UpdateMGainPayment")]
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

        #region Delete MGain Payment Details
        [HttpDelete("DeleteMGainPayment")]
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
