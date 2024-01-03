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

        #region Get All MGain Scheme Details
        [HttpGet("GetMGainSchemeDetails")]
        public async Task<IActionResult> GetMGainScheme(bool? IsCumulative, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var mGainSchemes = await _mGainSchemeService.GetMGainSchemeDetailsAsync(IsCumulative, search, sortingParams);
                return Ok(mGainSchemes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add MGain Scheme
        [HttpPost("AddMGainScheme")]
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

        #region Update MGain Scheme
        [HttpPut("UpdateMGainScheme")]
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
