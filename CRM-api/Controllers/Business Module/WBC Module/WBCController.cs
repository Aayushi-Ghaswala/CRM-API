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
        public async Task<IActionResult> ReleaseGP(DateTime date)
        {
            try
            {
                var gp = await _wbcService.ReleaseGPAsync(date);
                return gp != 0 ? Ok(new { Message = "Gold point released successfully." }) : BadRequest(new { Message = "Unable to release gold point"});
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

        #region Get all subInvestment types
        [HttpGet("GetAllSubInvestmentTypes")]
        public async Task<IActionResult> GetAllSubInvestmentTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var schemes = await _wbcService.GetAllSubInvestmentTypesAsync(search, sortingParams);
                return Ok(schemes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get all subSubInvestment types
        [HttpGet("GetAllSubSubInvestmentTypes")]
        public async Task<IActionResult> GetAllSubSubInvestmentTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, int? subInvestmentTypeId)
        {
            try
            {
                var schemes = await _wbcService.GetAllSubSubInvestmentTypesAsync(search, sortingParams, subInvestmentTypeId);
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
