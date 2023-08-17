using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.WBC_Mall_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MallProductController : ControllerBase
    {
        private readonly IMallProductService _mallProductService;

        public MallProductController(IMallProductService mallProductService)
        {
            _mallProductService = mallProductService;
        }

        #region Get Mall Products
        [HttpGet("GetMallProducts")]
        public async Task<IActionResult> GetMallProducts(int? catId, string? filterString, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var products = await _mallProductService.GetMallProductsAsync(catId, filterString, search, sortingParams);
                return Ok(products);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Mall Product
        [HttpPost("AddMallProduct")]
        public async Task<ActionResult> AddMallProduct([FromForm] AddMallProductDto addMallProductDto)
        {
            try
            {
                var flag = await _mallProductService.AddMallProductAsync(addMallProductDto);
                return flag != 0 ? Ok(new { Message = "Product added successfully." }) : BadRequest(new { Message = "Unable to add product." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Mall Product
        [HttpPut("UpdateMallProduct")]
        public async Task<ActionResult> UpdateMallProduct([FromForm] UpdateMallProductDto updateMallProductDto)
        {
            try
            {
                var flag = await _mallProductService.UpdateMallProductAsync(updateMallProductDto);
                return flag != 0 ? Ok(new { Message = "Poduct updated successfully." }) : BadRequest(new { Message = "Unable to update product." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Mall Product Image
        [HttpDelete("DeleteProductImage")]
        public async Task<ActionResult> DeleteProductImage(int id)
        {
            try
            {
                var flag = await _mallProductService.DeleteProductImageAsync(id);
                return flag != 0 ? Ok(new { Message = "Product image deleted successfully." }) : BadRequest(new { Message = "Unable to delete product image." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
