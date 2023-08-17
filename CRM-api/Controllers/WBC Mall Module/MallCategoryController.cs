using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.WBC_Mall_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MallCategoryController : ControllerBase
    {
        private readonly IMallCategoryService _mallCategoryService;

        public MallCategoryController(IMallCategoryService mallCategoryService)
        {
            _mallCategoryService = mallCategoryService;
        }

        #region Get Mall Categories
        [HttpGet("GetMallCategories")]
        public async Task<IActionResult> GetMallCategories(string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var categories = await _mallCategoryService.GetMallCategoriesAsync(search, sortingParams);
                return Ok(categories);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Mall Category
        [HttpPost("AddMallCategory")]
        public async Task<ActionResult> AddMallCategory([FromForm] AddMallCategoryDto addMallCategory)
        {
            try
            {
                var flag = await _mallCategoryService.AddMallCategoryAsync(addMallCategory);
                return flag != 0 ? Ok(new { Message = "Mall category added successfully." }) : BadRequest(new { Message = "Unable to add mall category." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Mall Category
        [HttpPut("UpdateMallCategory")]
        public async Task<ActionResult> UpdateMallCategory([FromForm] UpdateMallCategoryDto updateMallCategory)
        {
            try
            {
                var flag = await _mallCategoryService.UpdateMallCategoryAsync(updateMallCategory);
                return flag != 0 ? Ok(new { Message = "Mall category updated successfully." }) : BadRequest(new { Message = "Unable to update mall category." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region De-Activate Mall Category
        [HttpDelete("DeActivateMallCategory")]
        public async Task<ActionResult> DeActivateMallCategory(int id)
        {
            try
            {
                var flag = await _mallCategoryService.DeActivateMallCategoryAsync(id);
                return flag == 1 ? Ok(new { Message = "Mall category deleted succesfully." }) : flag == 0 ? BadRequest(new { Message = "Unable to delete mall category." })
                                 : BadRequest(new { Message = "Mall category in use." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
