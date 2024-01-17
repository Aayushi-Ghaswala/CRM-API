using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.AddDataDto.Real_Estate_Module;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Real_Estate_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlotController : ControllerBase
    {
        private readonly IPlotService _plotService;

        public PlotController(IPlotService plotService)
        {
            _plotService = plotService;
        }

        #region Get Plot
        [HttpGet("GetPlot")]
        public async Task<IActionResult> GetPlot(int? projectId, string? purpose, string? assignStatus, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var plot = await _plotService.GetPlotAsync(projectId, purpose, search, sortingParams, assignStatus);
                return Ok(plot);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Plot
        [HttpPost("AddPlot")]
        public async Task<IActionResult> AddPlot(AddPlotDto addPlot)
        {
            try
            {
                var plot = await _plotService.AddPlotAsync(addPlot);
                return plot != 0 ? Ok(new { Message = "Plot added successfully." }) : BadRequest(new { Message = "Unable to add plot." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Plot
        [HttpPut("UpdatePlot")]
        public async Task<IActionResult> UpdatePlot(UpdatePlotDto updatePlot)
        {
            try
            {
                var plot = await _plotService.UpdatePlotAsync(updatePlot);
                return plot != 0 ? Ok(new { Message = "Plot updated successfully." }) : BadRequest(new { Message = "Unable to update plot" });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Plot
        [HttpDelete("DeletePlot")]
        public async Task<IActionResult> DeletePlot(int id)
        {
            try
            {
                var plot = await _plotService.DeletePlotAsync(id);
                return plot != 0 ? Ok(new { Message = "Plot deleted successfully." }) : BadRequest(new { Message = "Unable to delete Plot." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion 
    }
}
