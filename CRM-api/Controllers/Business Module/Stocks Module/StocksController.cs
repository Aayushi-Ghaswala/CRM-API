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
        public async Task<IActionResult> GetStocksUsersName(string? scriptName, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var result = await _stockService.GetStocksUsersNameAsync(scriptName, search, sortingParams);
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
        public async Task<IActionResult> GetAllScriptNames([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, string? clientName = null)
        {
            try
            {
                var scriptNames = await _stockService.GetAllScriptNamesAsync(clientName, search, sortingParams);
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
        public async Task<IActionResult> GetStocksData([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, string? clientName = null, DateTime? fromDate = null, DateTime? toDate = null, string? scriptName = null, string? firmName = null)
        {
            try
            {
                var stocksData = await _stockService.GetStockDataAsync(clientName, fromDate, toDate, scriptName, firmName, search, sortingParams);
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
                var res = await _stockService.ImportSharekhanTradeFileAsync(formFile, firmName, 0, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Sharekhan Individual Trade File[.csv]
        [HttpPost("ImportSharekhanIndividualTradeFile")]
        public async Task<IActionResult> ImportSharekhanIndividualTradeFile(IFormFile formFile, string firmName, int id, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportSharekhanTradeFileAsync(formFile, firmName, id, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Jainam Individual Trade File[.xls]
        [HttpPost("ImportJainamIndividualTradeFile")]
        public async Task<IActionResult> ImportJainamIndividualTradeFile(IFormFile formFile, string firmName, string clientName, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportJainamTradeFileAsync(formFile, firmName, clientName, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
