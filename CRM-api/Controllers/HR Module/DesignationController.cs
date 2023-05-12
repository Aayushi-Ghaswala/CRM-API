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
        [HttpGet]
        public async Task<ActionResult<DisplayDesignationDto>> GetDesignation(int page)
        {
            try
            {
                var designations = await _designationService.GetDesignation(page);
                return designations.Values.Count > 0 ? Ok(designations) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Designation By Id
        [HttpGet("GetDesignationById")]
        public async Task<ActionResult<DesignationDto>> GetDesignationById(int desigId)
        {
            try
            {
                var designation = await _designationService.GetDesignationById(desigId);
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
                return row > 0 ? Ok("Added") : BadRequest("Unable to Add");
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
                return row > 0 ? Ok("Updated") : BadRequest("Unable to update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Designation by Department
        [HttpGet("GetDesignationByDepartment")]
        public async Task<ActionResult> GetDesignationByDepartment(int id)
        {
            try
            {
                var designations = await _designationService.GetDesignationByDepartment(id);
                return designations.Count() > 0 ? Ok(designations) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
