using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module;
using CRM_api.Services.IServices.Business_Module.LI_GI_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.LI_GI_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class InsuranceClientController : Controller
    {
        private readonly IInsuranceClientService _insuranceClientService;

        public InsuranceClientController(IInsuranceClientService insuranceClientService)
        {
            _insuranceClientService = insuranceClientService;
        }

        #region Get All Insurance Client Details
        [HttpGet("GetInsuranceClients")]
        public async Task<IActionResult> GetInsuranceClients([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var insClients = await _insuranceClientService.GetInsuranceClientsAsync(search, sortingParams);
                return Ok(insClients);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Insurance Company List By InsType Id
        [HttpGet("GetCompanyListByInsTypeId")]
        public async Task<IActionResult> GetCompanyListByInsTypeId(int id, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var insCompanies = await _insuranceClientService.GetCompanyListByInsTypeIdAsync(id, sortingParams);
                return insCompanies.Values.Count >= 0 ? Ok(insCompanies) : BadRequest(new { Message = "Insurance company not found." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Insurance Client By Id
        [HttpGet("GetInsuranceClientById")]
        public async Task<IActionResult> GetInsuranceClientById(int id)
        {
            try
            {
                var insClient = await _insuranceClientService.GetInsuranceClientByIdAsync(id);
                return insClient is not null ? Ok(insClient) : BadRequest(new { Message = "Insurance client not found." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Insurance Client Detail
        [HttpPost("AddInsuranceClientDetail")]
        public async Task<ActionResult> AddInsuranceClientDetail(AddInsuranceClientDto insuranceClientDto)
        {
            try
            {
                var flag = await _insuranceClientService.AddInsuranceClientAsync(insuranceClientDto);
                return flag != 0 ? Ok(new { Message = "Insurance client added successfully." }) : BadRequest(new { Message = "Unable to add insurance client" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update Insurance Client Detail
        [HttpPut("UpdateInsuranceClientDetail")]
        public async Task<ActionResult> UpdateInsuranceClientDetail(UpdateInsuranceClientDto insuranceClientDto)
        {
            try
            {
                var flag = await _insuranceClientService.UpdateInsuranceClientAsync(insuranceClientDto);
                return flag != 0 ? Ok(new { Message = "Insurance client updated successfully." }) : BadRequest(new { Message = "Unable to update insurance client" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Deactivate Insurance Client Detail
        [HttpDelete("DeactivateInsClient")]
        public async Task<ActionResult> DeactivateInsClient(int id)
        {
            try
            {
                var flag = await _insuranceClientService.DeactivateInsClientAsync(id);
                return flag != 0 ? Ok(new { Message = "Insurance client deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate insurance client." }); 
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
