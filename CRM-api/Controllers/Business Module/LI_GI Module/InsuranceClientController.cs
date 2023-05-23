using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.LI_GI_Module;
using CRM_api.Services.IServices.Business_Module.LI_GI_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRM_api.Controllers.Business_Module.LI_GI_Module
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InsuranceClientController : Controller
    {
        private readonly IInsuranceClientService _insuranceClientService;

        public InsuranceClientController(IInsuranceClientService insuranceClientService)
        {
            _insuranceClientService = insuranceClientService;
        }

        [HttpGet]
        #region Get All Insurance Client Details
        public async Task<IActionResult> GetInsuranceClients([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
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

                var insClients = await _insuranceClientService.GetInsuranceClientsAsync(data, sortingParams);

                return Ok(insClients);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Insurance Company List By InsType Id
        public async Task<IActionResult> GetCompanyListByInsTypeId(int id, int page)
        {
            try
            {
                var insCompanies = await _insuranceClientService.GetCompanyListByInsTypeIdAsync(id, page);
                return insCompanies.Values.Count >= 0 ? Ok(insCompanies) : BadRequest(new { Message = "Insurance company not found." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get Insurance Client By Id
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

        [HttpPost]
        #region Add Insurance Client Detail
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

        [HttpPut]
        #region Update Insurance Client Detail
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

        [HttpDelete]
        #region Deactivate Insurance Client Detail
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
