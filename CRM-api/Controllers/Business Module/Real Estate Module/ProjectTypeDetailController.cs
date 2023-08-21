using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Real_Estate_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTypeDetailController : ControllerBase
    {
        private readonly IProjectTypeDetailService _projectTypeDetailService;

        public ProjectTypeDetailController(IProjectTypeDetailService projectTypeDetailService)
        {
            _projectTypeDetailService = projectTypeDetailService;
        }

        #region Get Project Type Details
        [HttpGet("GetProjectTypeDetails")]
        public async Task<IActionResult> GetProjectTypeDetails(int? projectTypeId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var projectTypeDetails = await _projectTypeDetailService.GetProjectTypeDetailsAsync(projectTypeId, search, sortingParams);
                return Ok(projectTypeDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Project Type Details
        [HttpGet("GetProjectTypes")]
        public async Task<IActionResult> GetProjectTypes(string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var projectTypes = await _projectTypeDetailService.GetProjectTypesAsync(search, sortingParams);
                return Ok(projectTypes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Project Type Detail
        [HttpPost("AddProjectTypeDetail")]
        public async Task<ActionResult> AddProjectTypeDetail(AddProjectTypeDetailDto addProjectTypeDetailDto)
        {
            try
            {
                var flag = await _projectTypeDetailService.AddProjectTypeDetailAsync(addProjectTypeDetailDto);
                return flag != 0 ? Ok(new { Message = "Project type detail added successfully." }) : BadRequest(new { Message = "Unable to add project type detail." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Project Type Detail
        [HttpPut("UpdateProjectTypeDetail")]
        public async Task<ActionResult> UpdateProjectTypeDetail(UpdateProjectTypeDetailDto updateProjectTypeDetailDto)
        {
            try
            {
                var flag = await _projectTypeDetailService.UpdateProjectTypeDetailAsync(updateProjectTypeDetailDto);
                return flag != 0 ? Ok(new { Message = "Project type detail updated successfully." }) : BadRequest(new { Message = "Unable to update project type detail." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Project Type Detail
        [HttpDelete("DeleteProjectTypeDetail")]
        public async Task<ActionResult> DeleteProjectTypeDetail(int id)
        {
            try
            {
                var flag = await _projectTypeDetailService.DeleteProjectTypeDetailAsync(id);
                return flag != 0 ? Ok(new { Message = "Project type detail deleted successfully." }) : BadRequest(new { Message = "Unable to delete project type detail." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
