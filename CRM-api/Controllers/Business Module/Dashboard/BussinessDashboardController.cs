using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard;
using CRM_api.Services.IServices.Business_Module.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class BussinessDashboardController : ControllerBase
    {
        private readonly IBusinessDashboardService _businessDashboardService;

        public BussinessDashboardController(IBusinessDashboardService businessDashboardService)
        {
            _businessDashboardService = businessDashboardService;
        }

        #region Get Client Current Investment Snapshot
        [HttpGet("GetClientCurrentInvSnapshot")]
        public async Task<IActionResult> GetClientCurrentInvSnapshot(bool? isZero, string? search, int? userId)
        {
            try
            {
                var getData = await _businessDashboardService.GetClientCurrentInvSnapshotAsync(userId, isZero, search);

                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Client Monthly Transaction Snapshot
        [HttpGet("GetClientMonthlyTransSnapshot")]
        public async Task<IActionResult> GetClientMonthlyTransSnapshot(int? userId, int? month, int? year, bool? isZero, string? search)
        {
            try
            {
                var getData = await _businessDashboardService.GetClientMonthlyTransSnapshotAsync(userId, month, year, isZero, search);

                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Client Monthly Chart
        [HttpGet("GetMonthlyChart")]
        public async Task<IActionResult> GetMonthlyChart()
        {
            try
            {
                var getData = await _businessDashboardService.GetMonthlyChartAsync();

                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Send Email Client Report Snapshots
        [HttpPost("SendCurrentInvSnapshotEmail")]
        public IActionResult SendCurrentInvSnapshotEmail(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto)
        {
            try
            {
                var flag = _businessDashboardService.SendCurrentInvSnapshotEmailAsync(clientReportDto);

                return flag == 1 ? Ok(new { Message = "Email send successfully." }) : BadRequest(new { Message = "Unable to send email." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Send Email Client Report Snapshots
        [HttpPost("SendCurrentInvSnapshotSMS")]
        public IActionResult SendCurrentInvSnapshotSMS(ClientReportDto<ClientCurrentInvSnapshotDto> clientReportDto)
        {
            try
            {
                var flag = _businessDashboardService.SendCurrentInvSnapshotSMSAsync(clientReportDto);

                return flag == 1 ? Ok(new { Message = "SMS send successfully." }) : BadRequest(new { Message = "Unable to send sms." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Send Email Client Report Snapshots
        [HttpPost("SendMonthlyTransSnapshotEmail")]
        public IActionResult SendMonthlyTransSnapshotEmail(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto)
        {
            try
            {
                var flag = _businessDashboardService.SendMonthlyTransSnapshotEmailAsync(clientReportDto);

                return flag == 1 ? Ok(new { Message = "Email send successfully." }) : BadRequest(new { Message = "Unable to send email." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Send Email Client Report Snapshots
        [HttpPost("SendMonthlyTransSnapshotSMS")]
        public IActionResult SendMonthlyTransSnapshotSMS(ClientReportDto<ClientMonthlyTransSnapshotDto> clientReportDto)
        {
            try
            {
                var flag = _businessDashboardService.SendMonthlyTransSnapshotSMSAsync(clientReportDto);

                return flag == 1 ? Ok(new { Message = "SMS send successfully." }) : BadRequest(new { Message = "Unable to send sms." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
