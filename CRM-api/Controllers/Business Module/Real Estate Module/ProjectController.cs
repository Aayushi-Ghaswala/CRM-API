using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Real_Estate_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _ProjectService;

        public ProjectController(IProjectService ProjectService)
        {
            _ProjectService = ProjectService;
        }

        #region Get Project
        [HttpGet("GetProject")]
        public async Task<IActionResult> GetProject(bool? isActive, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var project = await _ProjectService.GetProjectAsync(isActive, search, sortingParams);
                return Ok(project);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Project
        [HttpPost("AddProject")]
        public async Task<IActionResult> AddProject(AddProjectDto addProject)
        {
            try
            {
                var project = await _ProjectService.AddProjectAsync(addProject);
                return project != 0 ? Ok(new { Message = "Project added successfully." }) : BadRequest(new { Message = "Unable to add project." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Project
        [HttpPut("UpdateProject")]
        public async Task<IActionResult> UpdateProject(UpdateProjectDto updateProject)
        {
            try
            {
                var project = await _ProjectService.UpdateProjectAsync(updateProject);
                return project != 0 ? Ok(new { Message = "Project updated successfully." }) : BadRequest(new { Message = "Unable to update project" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Project
        [HttpDelete("DeactivateProject")]
        public async Task<IActionResult> DeactivateProject(int id)
        {
            try
            {
                var project = await _ProjectService.DeactivateProjectAsync(id);
                return project != 0 ? Ok(new { Message = "Project deactivate successfully." }) : BadRequest(new { Message = "Unable to deactivate project." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 
    }
}
