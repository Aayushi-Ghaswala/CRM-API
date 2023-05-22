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
        [HttpGet]
        public async Task<IActionResult> GetDepartment([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
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
                var departments = await _departmentService.GetDepartmentAsync(data, sortingParams);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Department By Id
        [HttpGet("GetDepartmentById")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int deptId)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(deptId);
                return department.DepartmentId != 0 ? Ok(department) : NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add Department
        [HttpPost]
        public async Task<ActionResult> AddDepartment(AddDepartmentDto addDepartmentDto)
        {
            try
            {
                int row = await _departmentService.AddDepartmentAsync(addDepartmentDto);
                return row > 0 ? Ok(new { Message = "Department added successfully."}) : BadRequest(new { Message = "Unable to add department."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update Department
        [HttpPut]
        public async Task<ActionResult> UpdateDepartment(UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                int row = await _departmentService.UpdateDepartmentAsync(updateDepartmentDto);
                return row != 0 ? Ok(new { Message = "Department updated successfully."}) : BadRequest(new { Message = "Unable to update department."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Deactivate department
        [HttpDelete]
        public async Task<IActionResult> DeactivateDepartment(int id)
        {
            var department = _departmentService.DeactivateDepartmentAsync(id);
            return await department !=0 ? Ok(new { Message = "Department deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate department."});
        }
        #endregion
    }
}
