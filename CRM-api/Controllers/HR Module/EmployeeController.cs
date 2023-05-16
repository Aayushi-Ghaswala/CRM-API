using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
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
        [HttpGet]
        public async Task<ActionResult<ResponseDto<UserMasterDto>>> GetEmployees(int page)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(page);
                return employees.Values.Count > 0 ? Ok(employees) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Employee by Id
        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult<UserMasterDto>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeById(id);
                return employee.UserId != 0 ? Ok(employee) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return row > 0 ? Ok("Employee added successfully.") : BadRequest("Unable to add employee.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                return row !=0 ? Ok("Employee updated successfully.") : BadRequest("Unable to update employee.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate employee
        [HttpDelete]
        public async Task<IActionResult> DeactivateEmployee(int id)
        {
            int row = await _employeeService.DeactivateEmployeeAsync(id);
            return row != 0 ? Ok("Employee deactivated successfully.") : BadRequest("Unable to deactivate employee");
        }
        #endregion
    }
}
