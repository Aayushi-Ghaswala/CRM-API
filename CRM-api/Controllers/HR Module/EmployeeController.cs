using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        [HttpGet]
        public async Task<ActionResult<ResponseDto<UserMasterDto>>> GetEmployees([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
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

        #region Get Employee by Id
        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult<UserMasterDto>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                return employee.UserId != 0 ? Ok(employee) : NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add employee
        [HttpPost]
        public async Task<ActionResult> AddEmployee(AddUserMasterDto addUserMasterDto)
        {
            try
            {
                int row = await _employeeService.AddEmployeeAsync(addUserMasterDto);
                return row > 0 ? Ok(new { Message = "Employee added successfully."}) : BadRequest(new { Message = "Unable to add employee."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update employee
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(UpdateUserMasterDto updateUserMasterDto)
        {
            try
            {
                int row = await _employeeService.UpdateEmployeeAsync(updateUserMasterDto);
                return row !=0 ? Ok(new { Message = "Employee updated successfully."}) : BadRequest(new { Message = "Unable to update employee."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate employee
        [HttpDelete]
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
    }
}
