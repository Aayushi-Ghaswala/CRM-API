using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.HR_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #region Get all emplloyees
        [HttpGet("GetEmployees")]
        public async Task<ActionResult<ResponseDto<EmployeeMasterDto>>> GetEmployees([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(search, sortingParams);
                return Ok(employees);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add employee
        [HttpPost("AddEmployee")]
        public async Task<ActionResult> AddEmployee(AddEmployeeDto addEmployeerDto)
        {
            try
            {
                var row = await _employeeService.AddEmployeeAsync(addEmployeerDto);
                return row.Item1 > 0 ? Ok(new { Message = row.Item2 }) : BadRequest(new { Message = row.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update employee
        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                int row = await _employeeService.UpdateEmployeeAsync(updateEmployeeDto);
                return row != 0 ? Ok(new { Message = "Employee updated successfully." }) : BadRequest(new { Message = "Unable to update employee." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate employee
        [HttpDelete("DeactivateEmployee")]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            try
            {
                int row = await _employeeService.DeactivateEmployeeAsync(id);
                return row != 0 ? Ok(new { Message = "Employee deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate employee." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        
        #region Delete employee Qualification
        [HttpDelete("DeactivateEmployeeQualification")]
        public async Task<IActionResult> DeleteEmployeeQualification(int id)
        {
            try
            {
                int row = await _employeeService.DeleteEmployeeQualificationAsync(id);
                return row != 0 ? Ok(new { Message = "Employee qualification deleted successfully." }) : BadRequest(new { Message = "Unable to delete employee qualification." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        
        #region Deactivate employee Experience
        [HttpDelete("DeactivateEmployeeExperience")]
        public async Task<IActionResult> DeactivateEmployeeExperience(int id)
        {
            try
            {
                int row = await _employeeService.DeleteEmployeeExperienceAsync(id);
                return row != 0 ? Ok(new { Message = "Employee experience deleted successfully." }) : BadRequest(new { Message = "Unable to delete employee experience." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
