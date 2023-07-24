using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.IServices.Business_Module.Fasttrack_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Fasttrack_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class FasttrackController : ControllerBase
    {
        private readonly IFasttrackService _fasttrackService;

        public FasttrackController(IFasttrackService fasttrackService)
        {
            _fasttrackService = fasttrackService;
        }
        #region Get Fasttrack Benefits
        [HttpGet("GetFasttrackBenefits")]
        public async Task<IActionResult> GetFasttrackBenefits()
        {
            try
            {
                return Ok(await _fasttrackService.GetFasttrackBenefitsAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Fasttrack Schemes
        [HttpGet("GetFasttrackSchemes")]
        public async Task<IActionResult> GetFasttrackSchemes()
        {
            try
            {
                return Ok(await _fasttrackService.GetFasttrackSchemesAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Fasttrack Benefits
        [HttpGet("GetFasttrackLevelCommission")]
        public async Task<IActionResult> GetFasttrackCommission()
        {
            try
            {
                return Ok(await _fasttrackService.GetFasttrackLevelCommissionAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Fasttrack Benefits
        [HttpPost("AddFasttrackBenefits")]
        public async Task<IActionResult> AddFasttrackBenefits(AddFasttrackBenefitsDto addFasttrackBenefits)
        {
            try
            {
                var benefits = await _fasttrackService.AddFasttrackBenefitsAsync(addFasttrackBenefits);
                return benefits != 0 ? Ok(new { Message = "Fasttrack benefit added successfully." }) : BadRequest(new { Message = "Unable to add fasttrack benefit." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Fasttrack Benefits
        [HttpPut("UpdateFasttrackBenefits")]
        public async Task<IActionResult> UpdateFasttrackBenefits(UpdateFasttrackBenefitsDto updateFasttrackBenefits)
        {
            try
            {
                var benefits = await _fasttrackService.UpdateFasttrackBenefitsAsync(updateFasttrackBenefits);
                return benefits != 0 ? Ok(new { Message = "Fasttrack benefit updated successfully." }) : BadRequest(new { Message = "Unable to update fasttrack Benefit." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update fasttrack scheme
        [HttpPut("UpdateFasttrackScheme")]
        public async Task<IActionResult> UpdateFasttrackScheme(UpdateFasttrackSchemeDto updateFasttrackSchemeDto)
        {
            try
            {
                var result = await _fasttrackService.UpdateFasttrackSchemeAsync(updateFasttrackSchemeDto);
                return result != 0 ? Ok(new { Message = "Fasttrack scheme updated successfully." }) : BadRequest(new { Message = "Unable to update fasttrack scheme." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update fasttrack level commission
        [HttpPut("UpdateFasttrackLevelCommission")]
        public async Task<IActionResult> UpdateFasttrackLevelCommission(UpdateFasttrackLevelCommissionDto levelCommissionDto)
        {
            try
            {
                var result = await _fasttrackService.UpdateFasttrackLevelsCommissionAsync(levelCommissionDto);
                return result != 0 ? Ok(new { Message = "Fasttrack level commission updated successfully." }) : BadRequest(new { Message = "Unable to update fasttrack level commission." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
