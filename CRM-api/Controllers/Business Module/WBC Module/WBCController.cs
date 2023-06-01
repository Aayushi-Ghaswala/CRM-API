using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.IServices.WBC_Module;
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
                return result != 0 ? Ok(new { Message = "WBC scheme added successfully" }) : BadRequest(new { Message = "Unable to add wbc scheme" });
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
                return result != 0 ? Ok(new { Message = "WBC scheme updated successfully" }) : BadRequest(new { Message = "Unable to update wbc scheme" });
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
                return result != 0 ? Ok(new { Message = "WBC scheme deleted successfully" }) : BadRequest(new { Message = "Unable to delete wbc scheme" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
