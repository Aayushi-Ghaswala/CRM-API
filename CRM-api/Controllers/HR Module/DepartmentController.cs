using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetDepartment(int page)
        {
            try
            {
                var departments = await _departmentService.GetDepartmentAsync(page);
                return departments.Values.Count > 0 ? Ok(departments) : NoContent();
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
                return row > 0 ? Ok("Added") : BadRequest("Unable to Add");
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
                return row > 0 ? Ok("Updated") : BadRequest("Unable to update");
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
    }
}
