using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        #region Get all Sources
        [HttpGet("GetSources")]
        public async Task<IActionResult> GetSources([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var source = await _sourceService.GetSourcesAsync(search, sortingParams);
                return Ok(source);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Source By Id
        [HttpGet("GetSourceById")]
        public async Task<IActionResult> GetSourceById(int sourceId)
        {
            try
            {
                var source = await _sourceService.GetSourceByIdAsync(sourceId);
                return source.Id != 0 ? Ok(source) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Source By Name
        [HttpGet("GetSourceByName")]
        public async Task<IActionResult> GetSourceByName(string Name)
        {
            try
            {
                var source = await _sourceService.GetSourceByNameAsync(Name);
                return source.Id != 0 ? Ok(source) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Source
        [HttpPost("AddSource")]
        public async Task<IActionResult> AddSource(AddSourceDto addSourceDto)
        {
            try
            {
                int row = await _sourceService.AddSourceAsync(addSourceDto);
                return row > 0 ? Ok(new { Message = "Source added successfully."}) : BadRequest(new { Message = "Unable to add source."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Source
        [HttpPut("UpdateSource")]
        public async Task<IActionResult> UpdateSource(UpdateSourceDto updateSourceDto)
        {
            try
            {
                int row = await _sourceService.UpdateSourceAsync(updateSourceDto);
                return row > 0 ? Ok(new { Message = "Source updated successfully."}) : BadRequest(new { Message = "Unable to update source."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Source
        [HttpDelete("DeactivateSource")]
        public async Task<IActionResult> DeactivateSource(int id)
        {
            try
            {
                var source = await _sourceService.DeactivateSourceAsync(id);
                return source != 0 ? Ok(new { Message = "Source deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate source." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
