using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Investment_Module;
using CRM_api.Services.IServices.Business_Module.Insvestment_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Insvestment_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentController : ControllerBase
    {
        private readonly IInvestmentService _investmentService;

        public InvestmentController(IInvestmentService investmentService)
        {
            _investmentService = investmentService;
        }

        #region Get InvestmentType
        [HttpGet("GetInvestmentType")]
        public async Task<IActionResult> GetInvestmentType(string? search, [FromQuery]SortingParams sortingParams, bool? isActive)
        {
            try
            {
                var result = await _investmentService.GetInvestmentTypeAsync(search, sortingParams, isActive);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get InvestmentType by id
        [HttpGet("GetInvestmentTypeById")]
        public async Task<IActionResult> GetInvestmentTypeById(int id)
        {
            try
            {
                var result = await _investmentService.GetInvestmentTypeByIdAsync(id);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SubInvestmentType
        [HttpGet("GetSubInvestmentType")]
        public async Task<IActionResult> GetSubInvestmentType(string? search, [FromQuery] SortingParams sortingParams, bool? isActive)
        {
            try
            {
                var result = await _investmentService.GetSubInvestmentTypeAsync(search, sortingParams, isActive);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SubInvestmentType by InvestmentType id
        [HttpGet("GetSubInvestmentTypeByInvId")]
        public async Task<IActionResult> GetSubInvestmentTypeByInvId(int invId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var result = await _investmentService.GetSubInvestmentTypeByInvIdAsync(invId, search, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SubsubInvestmentType
        [HttpGet("GetSubsubInvestmentType")]
        public async Task<IActionResult> GetSubsubInvestmentType(string? search, [FromQuery] SortingParams sortingParams, bool? isActive)
        {
            try
            {
                var result = await _investmentService.GetSubsubInvestmentTypeAsync(search, sortingParams, isActive);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get SubsubInvestmentType by SubInvestmentType id
        [HttpGet("GetSubsubInvestmentTypeBySubInvId")]
        public async Task<IActionResult> GetSubsubInvestmentType(int id, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var result = await _investmentService.GetSubsubInvestmentTypeBySubInvIdAsync(id, search, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add InvestmentType
        [HttpPost("AddInvestmentType")]
        public async Task<IActionResult> AddInvestmentType(AddInvestmentTypeDto addInvestmentTypeDto)
        {
            try
            {
                var result = await _investmentService.AddInvestmentTypeAsync(addInvestmentTypeDto);
                return result != 0 ? Ok(new { Message = "InvestmentType Added successfully." }) : BadRequest(new { Message = "Unable to add InvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add SubInvestmentType
        [HttpPost("AddSubInvestmentType")]
        public async Task<IActionResult> AddSubInvestmentType(AddSubInvestmentTypeDto addSubInvestmentTypeDto)
        {
            try
            {
                var result = await _investmentService.AddSubInvestmentTypeAsync(addSubInvestmentTypeDto);
                return result != 0 ? Ok(new { Message = "Sub InvestmentType Added successfully." }) : BadRequest(new { Message = "Unable to add Sub InvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add SubsubInvestmentType
        [HttpPost("AddSubsubInvestmentType")]
        public async Task<IActionResult> AddSubsubInvestmentType(AddSubsubInvTypeDto addSubsubInvTypeDto)
        {
            try
            {
                var result = await _investmentService.AddSubsubInvestmentTypeAsync(addSubsubInvTypeDto);
                return result != 0 ? Ok(new { Message = "SubsubInvestmentType Added successfully." }) : BadRequest(new { Message = "Unable to add SubsubInvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update InvestmentType
        [HttpPut("UpdateInvestmentType")]
        public async Task<IActionResult> UpdateInvestmentType(UpdateInvestmentTypeDto updateInvestmentTypeDto)
        {
            try
            {
                var result = await _investmentService.UpdateInvestmentTypeAsync(updateInvestmentTypeDto);
                return result != 0 ? Ok(new { Message = "InvestmentType Updated successfully." }) : BadRequest(new { Message = "Unable to update InvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update SubInvestmentType
        [HttpPut("UpdateSubInvestmentType")]
        public async Task<IActionResult> UpdateSubInvestmentType(UpdateSubInvestmentTypeDto updateSubInvestmentTypeDto)
        {
            try
            {
                var result = await _investmentService.UpdateSubInvestmentTypeAsync(updateSubInvestmentTypeDto);
                return result != 0 ? Ok(new { Message = "Sub InvestmentType Updated successfully." }) : BadRequest(new { Message = "Unable to update sub InvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update SubsubInvestmentType
        [HttpPut("UpdateSubsubInvestmentType")]
        public async Task<IActionResult> UpdateSubsubInvestmentType(UpdateSubsubInvTypeDto updateSubsubInvTypeDto)
        {
            try
            {
                var result = await _investmentService.UpdateSubsubInvestmentTypeAsync(updateSubsubInvTypeDto);
                return result != 0 ? Ok(new { Message = "SubsubInvestmentType Updated successfully." }) : BadRequest(new { Message = "Unable to update SubsubInvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactive InvestmentType
        [HttpDelete("DeactiveInvestmentType")]
        public async Task<IActionResult> DeactiveInvestmentType(int id)
        {
            try
            {
                var result = await _investmentService.DeactiveInvestmentTypeAsync(id);
                return result != 0 ? Ok(new { Message = "InvestmentType Deactived successfully." }) : BadRequest(new { Message = "Unable to Deactivate InvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactive SubInvestmentType
        [HttpDelete("DeactiveSubInvestmentType")]
        public async Task<IActionResult> DeactiveSubInvestmentType(int id)
        {
            try
            {
                var result = await _investmentService.DeactiveSubInvestmentTypeAsync(id);
                return result != 0 ? Ok(new { Message = "SubInvestmentType Deactived successfully." }) : BadRequest(new { Message = "Unable to Deactivate SubInvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactive SubsubInvestmentType
        [HttpDelete("DeactiveSubsubInvestmentType")]
        public async Task<IActionResult> DeactiveSubsubInvestmentType(int id)
        {
            try
            {
                var result = await _investmentService.DeactiveSubsubInvestmentTypeAsync(id);
                return result != 0 ? Ok(new { Message = "SubsubInvestmentType Deactived successfully." }) : BadRequest(new { Message = "Unable to Deactivate SubsubInvestmentType." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
