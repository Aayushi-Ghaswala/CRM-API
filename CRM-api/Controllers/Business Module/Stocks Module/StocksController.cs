using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.Stocks_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService sharekhanStockService)
        {
            _stockService = sharekhanStockService;
        }

        #region Import Sharekhan All Trade File[.csv]
        [HttpPost("ImportSharekhanAllTradeFile")]
        public async Task<ActionResult> ImportSharekhanAllTradeFile(IFormFile formFile, string firmName, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportSharekhanTradeFile(formFile, firmName, 0, overrideData);
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
        public async Task<ActionResult> ImportSharekhanIndividualTradeFile(IFormFile formFile, string firmName, int id, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportSharekhanTradeFile(formFile, firmName, id, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Import Jainam Individual Trade File[.xls]
        [HttpPost("ImportJainamIndividualTradeFile")]
        public async Task<ActionResult> ImportJainamIndividualTradeFile(IFormFile formFile, string firmName, string clientName, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportJainamTradeFile(formFile, firmName, clientName, overrideData);
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
