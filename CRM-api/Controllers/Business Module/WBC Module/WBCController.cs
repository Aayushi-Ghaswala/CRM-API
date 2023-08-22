using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.IServices.Business_Module.WBC_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.WBC_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class WBCController : ControllerBase
    {
        private readonly IWBCService _wbcService;

        public WBCController(IWBCService wBCService)
        {
            _wbcService = wBCService;
        }

        #region Get gold point category
        [HttpGet("GetPointCategory")]
        public async Task<IActionResult> GetPointCategory()
        {
            try
            {
                var result = await _wbcService.GetPointCategoryAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get goldpoint user name
        [HttpGet("GetGPUsername")]
        public async Task<IActionResult> GetGPUsername(string? type, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var result = await _wbcService.GetGPUsernameAsync(type, search, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get goldpoint type
        [HttpGet("GetGPTypes")]
        public async Task<IActionResult> GetGPTypes(int? userId, string? searchingParams, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var result = await _wbcService.GetGPTypesAsync(userId, searchingParams, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get gold point ledger report
        [HttpGet("GetGPLedgerReport")]
        public async Task<IActionResult> GetGPLedgerReport(DateTime? date, int? userId, string? type, int? categoryId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var result = await _wbcService.GetGPLedgerReportAsync(date, userId, type, categoryId, search, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get wbc GP of month
        [HttpGet("GetGP")]
        public async Task<IActionResult> GetGoldPoint(string? search, DateTime date, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var wbc = await _wbcService.GetGPAsync(search, date, sortingParams);
                return Ok(wbc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Release Gold point
        [HttpGet("ReleaseGP")]
        public async Task<IActionResult> ReleaseGP(DateTime date, bool isTruncate = false)
        {
            try
            {
                var result = await _wbcService.ReleaseGPAsync(date, isTruncate);
                return result.Item1 > 0 ? Ok(new { Code = result.Item1, Message = result.Item2 }) : BadRequest(new { Code = result.Item1, Message = result.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get all Wbc scheme types
        [HttpGet("GetAllWbcSchemeTypes")]
        public async Task<IActionResult> GetAllWbcSchemeTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var schemes = await _wbcService.GetAllWbcSchemeTypesAsync(search, sortingParams);
                return Ok(schemes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get all wbc schemes
        [HttpGet("GetAllWbcSchemes")]
        public async Task<IActionResult> GetAllWbcSchemes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var schemes = await _wbcService.GetAllWbcSchemesAsync(search, sortingParams);
                return Ok(schemes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get direct refferals list
        [HttpGet("GetDirectRefferals")]
        public async Task<IActionResult> GetDirectRefferals(int userId, string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var result = await _wbcService.GetDirectRefferalsAsync(userId, search, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get referred by list
        [HttpGet("GetReferredByList")]
        public async Task<IActionResult> GetReferredByList(int? userId)
        {
            try
            {
                var result = await _wbcService.GetReferredByListAsync(userId);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add wbc scheme
        [HttpPost("AddWbcScheme")]
        public async Task<IActionResult> AddWbcScheme(AddWBCSchemeDto addWBCSchemeDto)
        {
            try
            {
                var result = await _wbcService.AddWbcSchemeAsync(addWBCSchemeDto);
                return result != 0 ? Ok(new { Message = "WBC scheme added successfully." }) : BadRequest(new { Message = "Unable to add wbc scheme." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update wbc scheme
        [HttpPut("UpdateWbcScheme")]
        public async Task<IActionResult> UpdateWbcScheme(UpdateWBCSchemeDto updateWBCSchemeDto)
        {
            try
            {
                var result = await _wbcService.UpdateWbcSchemeAsync(updateWBCSchemeDto);
                return result != 0 ? Ok(new { Message = "WBC scheme updated successfully." }) : BadRequest(new { Message = "Unable to update wbc scheme." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete wbc scheme
        [HttpDelete("DeleteWbcScheme")]
        public async Task<IActionResult> DeleteWbcScheme(int id)
        {
            try
            {
                var result = await _wbcService.DeleteWbcSchemeAsync(id);
                return result != 0 ? Ok(new { Message = "WBC scheme deleted successfully." }) : BadRequest(new { Message = "Unable to delete wbc scheme." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
