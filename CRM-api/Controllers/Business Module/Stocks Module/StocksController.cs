using CRM_api.DataAccess.Helper;
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

        #region Get stock user's names
        [HttpGet("GetStocksUsersName")]
        public async Task<IActionResult> GetStocksUsersName([FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var result = await _stockService.GetStocksUsersName(searchingParams, sortingParams);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get script names
        [HttpGet("GetAllScriptNames")]
        public async Task<IActionResult> GetAllScriptNames([FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams, string? clientName = null)
        {
            try
            {
                var scriptNames = await _stockService.GetAllScriptNames(clientName, searchingParams, sortingParams);
                return Ok(scriptNames);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get stocks data
        [HttpGet("GetStocksData")]
        public async Task<IActionResult> GetStocksData([FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams, string? clientName = null, DateTime? fromDate = null, DateTime? toDate = null, string? scriptName = null)
        {
            try
            {
                var stocksData = await _stockService.GetStockData(clientName, fromDate, toDate, scriptName, searchingParams, sortingParams);
                return Ok(stocksData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Sharekhan All Trade File[.csv]
        [HttpPost("ImportSharekhanAllTradeFile")]
        public async Task<IActionResult> ImportSharekhanAllTradeFile(IFormFile formFile, string firmName, bool overrideData = false)
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
        public async Task<IActionResult> ImportSharekhanIndividualTradeFile(IFormFile formFile, string firmName, int id, bool overrideData = false)
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
        public async Task<IActionResult> ImportJainamIndividualTradeFile(IFormFile formFile, string firmName, string clientName, bool overrideData = false)
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
