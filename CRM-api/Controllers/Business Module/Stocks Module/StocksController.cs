using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Stocks_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ISharekhanStockService _sharekhanStockService;

        public StocksController(ISharekhanStockService sharekhanStockService)
        {
            _sharekhanStockService = sharekhanStockService;
        }

        #region Import Sharekhan All Trade File[.csv]
        [HttpPost("ImportSharekhanAllTradeFile")]
        public async Task<ActionResult> ImportSharekhanAllTradeFile(IFormFile formFile, bool overrideData = false)
        {
            try
            {
                var res = await _sharekhanStockService.ImportTradeFile(formFile, 0, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Import Sharekhan Individual Trade File[.csv]
        [HttpPost("ImportSharekhanIndividualTradeFile")]
        public async Task<ActionResult> ImportSharekhanIndividualTradeFile(IFormFile formFile, int id, bool overrideData = false)
        {
            try
            {
                var res = await _sharekhanStockService.ImportTradeFile(formFile, id, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
