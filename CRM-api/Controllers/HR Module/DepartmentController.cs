using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRM_api.Controllers.HR_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        #region Get all Departments
        [HttpGet("GetDepartment")]
        public async Task<IActionResult> GetDepartment([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var departments = await _departmentService.GetDepartmentAsync(search, sortingParams);

                return Ok(departments);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Department
        [HttpPost("AddDepartment")]
        public async Task<ActionResult> AddDepartment(AddDepartmentDto addDepartmentDto)
        {
            try
            {
                int row = await _departmentService.AddDepartmentAsync(addDepartmentDto);
                return row > 0 ? Ok(new { Message = "Department added successfully."}) : BadRequest(new { Message = "Unable to add department."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Department
        [HttpPut("UpdateDepartment")]
        public async Task<ActionResult> UpdateDepartment(UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                int row = await _departmentService.UpdateDepartmentAsync(updateDepartmentDto);
                return row != 0 ? Ok(new { Message = "Department updated successfully."}) : BadRequest(new { Message = "Unable to update department."});
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate department
        [HttpDelete("DeactivateDepartment")]
        public async Task<IActionResult> DeactivateDepartment(int id)
        {
            try
            {
                var department = _departmentService.DeactivateDepartmentAsync(id);
                return await department != 0 ? Ok(new { Message = "Department deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate department." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
