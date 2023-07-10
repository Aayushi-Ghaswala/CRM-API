using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        #region Get all Statuss
        [HttpGet("GetStatues")]
        public async Task<IActionResult> GetStatues([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var status = await _statusService.GetStatuesAsync(search, sortingParams);
                return Ok(status);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Status By Id
        [HttpGet("GetStatusById")]
        public async Task<IActionResult> GetStatusById(int statusId)
        {
            try
            {
                var status = await _statusService.GetStatusByIdAsync(statusId);
                return status.Id != 0 ? Ok(status) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Status By Name
        [HttpGet("GetStatusByName")]
        public async Task<IActionResult> GetStatusByName(string Name)
        {
            try
            {
                var status = await _statusService.GetStatusByNameAsync(Name);
                return status.Id != 0 ? Ok(status) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Status
        [HttpPost("AddStatus")]
        public async Task<IActionResult> AddStatus(AddStatusDto addStatusDto)
        {
            try
            {
                int row = await _statusService.AddStatusAsync(addStatusDto);
                return row > 0 ? Ok(new { Message = "Status added successfully."}) : BadRequest(new { Message = "Unable to add status."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Status
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDto updateStatusDto)
        {
            try
            {
                int row = await _statusService.UpdateStatusAsync(updateStatusDto);
                return row > 0 ? Ok(new { Message = "Status updated successfully."}) : BadRequest(new { Message = "Unable to update status."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Status
        [HttpDelete("DeactivateStatus")]
        public async Task<IActionResult> DeactivateStatus(int id)
        {
            try
            {
                var status = await _statusService.DeactivateStatusAsync(id);
                return status != 0 ? Ok(new { Message = "Status deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate status." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
