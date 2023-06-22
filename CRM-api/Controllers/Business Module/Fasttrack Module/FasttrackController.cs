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
