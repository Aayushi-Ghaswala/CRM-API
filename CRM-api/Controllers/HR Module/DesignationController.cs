using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpGet]
        public async Task<IActionResult> GetDesignation([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var designations = await _designationService.GetDesignation(data, sortingParams);
                return Ok(designations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Designation By
        [HttpGet("GetDesignationByDepartment")]
        public async Task<ActionResult> GetDesignationByDepartment(int departmentId)
        {
            try
            {
                var designations = await _designationService.GetDesignationByDepartment(departmentId);
                return designations.Count() > 0 ? Ok(designations) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDesignationById")]
        public async Task<ActionResult<DesignationDto>> GetDesignationById(int id)
        {
            try
            {
                var designation = await _designationService.GetDesignationById(id);
                return designation.DesignationId != 0 ? Ok(designation) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Add Designation
        [HttpPost]
        public async Task<ActionResult> AddDesignation(AddDesignationDto addDesignationDto)
        {
            try
            {
                int row = await _designationService.AddDesignation(addDesignationDto);
                return row > 0 ? Ok(new { Message = "Designation added successfully"}) : BadRequest(new { Message = "Unable to add designation"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update Designation
        [HttpPut]
        public async Task<ActionResult> UpdateDesignation(UpdateDesignationDto updateDesignationDto)
        {
            try
            {
                int row = await _designationService.UpdateDesignation(updateDesignationDto);
                return row != 0 ? Ok(new { Message = "Designation updated successfully"}) : BadRequest(new { Message = "Unable to update designation"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate Designation
        [HttpDelete]
        public async Task<IActionResult> DeactivateDesignation(int id)
        {
            int row = await _designationService.DeactivateDesignation(id);
            return row !=0 ? Ok(new { Message = "Designation deactivated successfully"}) : BadRequest(new { Message = "Unable to deactivate designation"});
        }
        #endregion
    }
}
