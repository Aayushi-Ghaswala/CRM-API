using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.HR_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        #region Get all Designations
        [HttpGet("GetDesignation")]
        public async Task<IActionResult> GetDesignation([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var designations = await _designationService.GetDesignationAsync(search, sortingParams);
                return Ok(designations);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Designation
        [HttpPost("AddDesignation")]
        public async Task<ActionResult> AddDesignation(AddDesignationDto addDesignationDto)
        {
            try
            {
                int row = await _designationService.AddDesignationAsync(addDesignationDto);
                return row > 0 ? Ok(new { Message = "Designation added successfully" }) : BadRequest(new { Message = "Unable to add designation" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Designation
        [HttpPut("UpdateDesignation")]
        public async Task<ActionResult> UpdateDesignation(UpdateDesignationDto updateDesignationDto)
        {
            try
            {
                int row = await _designationService.UpdateDesignationAsync(updateDesignationDto);
                return row != 0 ? Ok(new { Message = "Designation updated successfully" }) : BadRequest(new { Message = "Unable to update designation" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Designation
        [HttpDelete("DeactivateDesignation")]
        public async Task<IActionResult> DeactivateDesignation(int id)
        {
            try
            {
                int row = await _designationService.DeactivateDesignationAsync(id);
                return row != 0 ? Ok(new { Message = "Designation deactivated successfully" }) : BadRequest(new { Message = "Unable to deactivate designation" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
