using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        #region Get all Campaigns
        [HttpGet("GetCampaigns")]
        public async Task<IActionResult> GetCampaigns([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignsAsync(search, sortingParams);
                return Ok(campaign);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Campaign By Id
        [HttpGet("GetCampaignById")]
        public async Task<IActionResult> GetCampaignById(int campaignId)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignByIdAsync(campaignId);
                return campaign.Id != 0 ? Ok(campaign) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Campaign By Name
        [HttpGet("GetCampaignByName")]
        public async Task<IActionResult> GetCampaignByName(string Name)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignByNameAsync(Name);
                return campaign.Id != 0 ? Ok(campaign) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Campaign
        [HttpPost("AddCampaign")]
        public async Task<IActionResult> AddCampaign(AddCampaignDto addCampaignDto)
        {
            try
            {
                int row = await _campaignService.AddCampaignAsync(addCampaignDto);
                return row > 0 ? Ok(new { Message = "Campaign added successfully."}) : BadRequest(new { Message = "Unable to add campaign."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Campaign
        [HttpPut("UpdateCampaign")]
        public async Task<IActionResult> UpdateCampaign(UpdateCampaignDto updateCampaignDto)
        {
            try
            {
                int row = await _campaignService.UpdateCampaignAsync(updateCampaignDto);
                return row > 0 ? Ok(new { Message = "Campaign updated successfully."}) : BadRequest(new { Message = "Unable to update campaign."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Campaign
        [HttpDelete("DeactivateCampaign")]
        public async Task<IActionResult> DeactivateCampaign(int id)
        {
            try
            {
                var campaign = await _campaignService.DeactivateCampaignAsync(id);
                return campaign != 0 ? Ok(new { Message = "Campaign deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate campaign." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
