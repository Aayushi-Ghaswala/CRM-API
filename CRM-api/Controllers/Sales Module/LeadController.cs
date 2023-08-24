using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Sales_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        #region Get all Leads
        [HttpGet("GetLeads")]
        public async Task<IActionResult> GetLeads([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, int? assignTo, bool export = false)
        {
            try
            {
                if (export)
                {
                    var leadCSV = await _leadService.GetLeadsForCSVAsync(assignTo, search, sortingParams);
                    return File(leadCSV, "text/csv", "Leads.csv");
                }
                else
                {
                    var lead = await _leadService.GetLeadsAsync(assignTo, search, sortingParams);
                    return Ok(lead);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get all Investment Types
        [HttpGet("GetInvestmentTypes")]
        public async Task<IActionResult> GetInvestmentTypes([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var investmentType = await _leadService.GetInvestmentTypesAsync(search, sortingParams);
                return Ok(investmentType);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Lead By Id
        [HttpGet("GetLeadById")]
        public async Task<IActionResult> GetLeadById(int leadId)
        {
            try
            {
                var lead = await _leadService.GetLeadByIdAsync(leadId);
                return lead.Id != 0 ? Ok(lead) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Lead By Name
        [HttpGet("GetLeadByName")]
        public async Task<IActionResult> GetLeadByName(string Name)
        {
            try
            {
                var lead = await _leadService.GetLeadByNameAsync(Name);
                return lead.Id != 0 ? Ok(lead) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Check MobileNo Exist in Lead
        [HttpGet("CheckMobileExist")]
        public ActionResult CheckMobileExist(int? id, string mobileNo)
        {
            try
            {
                var exist = _leadService.CheckMobileExistAsync(id, mobileNo);
                return exist != 0 ? Ok(exist) : BadRequest(new { Message = "Mobile number already exist." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Lead
        [HttpPost("AddLead")]
        public async Task<IActionResult> AddLead(AddLeadDto addLeadDto)
        {
            try
            {
                int row = await _leadService.AddLeadAsync(addLeadDto);
                return row > 0 ? Ok(new { Message = "Lead added successfully." }) : BadRequest(new { Message = "Unable to add lead." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Lead Data
        [HttpPost("ImportLead")]
        public async Task<IActionResult> ImportLead(IFormFile formFile)
        {
            try
            {
                var flag = await _leadService.ImportLeadAsync(formFile);
                return flag != 0 ? Ok(new { Message = "Lead file imported sucessfully." }) : BadRequest(new { Message = "Lead data already exist." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Lead
        [HttpPut("UpdateLead")]
        public async Task<IActionResult> UpdateLead(UpdateLeadDto updateLeadDto)
        {
            try
            {
                int row = await _leadService.UpdateLeadAsync(updateLeadDto);
                return row > 0 ? Ok(new { Message = "Lead updated successfully." }) : BadRequest(new { Message = "Unable to update lead." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Lead
        [HttpDelete("DeactivateLead")]
        public async Task<IActionResult> DeactivateLead(int id)
        {
            try
            {
                var lead = await _leadService.DeactivateLeadAsync(id);
                return lead != 0 ? Ok(new { Message = "Lead deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate lead." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Send Email Leads
        [HttpPost("SendLeadEmail")]
        public IActionResult SendLeadEmail(LeadDto leadDto)
        {
            try
            {
                var flag = _leadService.SendLeadEmailAsync(leadDto);

                return flag == 1 ? Ok(new { Message = "Email send successfully." }) : BadRequest(new { Message = "Unable to send email." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Send SMS Leads
        [HttpPost("SendLeadSMS")]
        public IActionResult SendLeadSMS(LeadDto leadDto)
        {
            try
            {
                var flag = _leadService.SendLeadSMSAsync(leadDto);

                return flag == 1 ? Ok(new { Message = "SMS send successfully." }) : BadRequest(new { Message = "Unable to send SMS." });
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
