using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceTypeController : ControllerBase
    {
        private readonly ISourceTypeService _sourceTypeService;

        public SourceTypeController(ISourceTypeService sourceTypeService)
        {
            _sourceTypeService = sourceTypeService;
        }

        #region Get all SourceTypes
        [HttpGet("GetSourceTypes")]
        public async Task<IActionResult> GetSourceTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var sourceType = await _sourceTypeService.GetSourceTypesAsync(search, sortingParams);
                return Ok(sourceType);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SourceType By Id
        [HttpGet("GetSourceTypeById")]
        public async Task<IActionResult> GetSourceTypeById(int sourceTypeId)
        {
            try
            {
                var sourceType = await _sourceTypeService.GetSourceTypeByIdAsync(sourceTypeId);
                return sourceType.Id != 0 ? Ok(sourceType) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SourceType By Name
        [HttpGet("GetSourceTypeByName")]
        public async Task<IActionResult> GetSourceTypeByName(string Name)
        {
            try
            {
                var sourceType = await _sourceTypeService.GetSourceTypeByNameAsync(Name);
                return sourceType.Id != 0 ? Ok(sourceType) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add SourceType
        [HttpPost("AddSourceType")]
        public async Task<IActionResult> AddSourceType(AddSourceTypeDto addSourceTypeDto)
        {
            try
            {
                int row = await _sourceTypeService.AddSourceTypeAsync(addSourceTypeDto);
                return row > 0 ? Ok(new { Message = "SourceType added successfully."}) : BadRequest(new { Message = "Unable to add sourceType."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update SourceType
        [HttpPut("UpdateSourceType")]
        public async Task<IActionResult> UpdateSourceType(UpdateSourceTypeDto updateSourceTypeDto)
        {
            try
            {
                int row = await _sourceTypeService.UpdateSourceTypeAsync(updateSourceTypeDto);
                return row > 0 ? Ok(new { Message = "SourceType updated successfully."}) : BadRequest(new { Message = "Unable to update sourceType."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate SourceType
        [HttpDelete("DeactivateSourceType")]
        public async Task<IActionResult> DeactivateSourceType(int id)
        {
            try
            {
                var sourceType = await _sourceTypeService.DeactivateSourceTypeAsync(id);
                return sourceType != 0 ? Ok(new { Message = "SourceType deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate sourceType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
