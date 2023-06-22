using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.IServices.Business_Module.MGain_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.MGain_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MGainSchemeController : ControllerBase
    {
        private readonly IMGainSchemeService _mGainSchemeService;

        public MGainSchemeController(IMGainSchemeService mGainSchemeService)
        {
            _mGainSchemeService = mGainSchemeService;
        }

        [HttpGet("GetMGainSchemeDetails")]
        #region Get All MGain Scheme Details
        public async Task<IActionResult> GetMGainScheme(bool? IsActive, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var mGainSchemes = await _mGainSchemeService.GetMGainSchemeDetailsAsync(IsActive, search, sortingParams);
                return Ok(mGainSchemes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost("AddMGainScheme")]
        #region Add MGain Scheme
        public async Task<IActionResult> AddMGainscheme(AddMGainSchemeDto MGainSchemeDto)
        {
            try
            {
                var mGain = await _mGainSchemeService.AddMGainSchemeAsync(MGainSchemeDto);
                return mGain != 0 ? Ok(new { Message = "MGain scheme added successfully." }) : BadRequest(new { Message = "MGain scheme already exists." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPut("UpdateMGainScheme")]
        #region Update MGain Scheme
        public async Task<IActionResult> UpdateMGainScheme(UpdateMGainSchemeDto updateMGainScheme)
        {
            try
            {
                var updateMGain = await _mGainSchemeService.UpdateMGainSchemeAsync(updateMGainScheme);
                return updateMGain != 0 ? Ok(new { Message = "MGain scheme updated successfully. " }) : BadRequest(new { Message = "Unable to update mgain scheme" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
