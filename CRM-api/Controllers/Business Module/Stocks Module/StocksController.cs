﻿using CRM_api.DataAccess.Helper;
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
        public async Task<IActionResult> GetStocksUsersName(string? scriptName, string? firmName, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var result = await _stockService.GetStocksUsersNameAsync(scriptName, firmName, search, sortingParams);
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
        public async Task<IActionResult> GetAllScriptNames(string? firmName, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams, string? clientName = null)
        {
            try
            {
                var scriptNames = await _stockService.GetAllScriptNamesAsync(clientName, firmName, search, sortingParams);
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
        public async Task<IActionResult> GetStocksData([FromQuery] string? search, [FromQuery] SortingParams? sortingParams, string? clientName = null, DateTime? fromDate = null, DateTime? toDate = null, string? scriptName = null, string? firmName = null, string? fileType = null)
        {
            try
            {
                var stocksData = await _stockService.GetStockDataAsync(clientName, fromDate, toDate, scriptName, firmName, fileType, search, sortingParams);
                return Ok(stocksData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get scriptwise client summary
        [HttpGet("GetClientwiseScriptSummary")]
        public async Task<IActionResult> GetClientwiseScriptSummary(string? userName, bool? isZero, DateTime? startDate, DateTime? endDate, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _stockService.GetClientwiseScripSummaryAsync(userName, isZero, startDate, endDate, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get clientwise stock summary
        [HttpGet("GetClientWiseSummary")]
        public async Task<IActionResult> GetClientWiseSummary(bool? isZero, DateTime? startDate, DateTime? endDate, [FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var getData = await _stockService.GetAllClientwiseStockSummaryAsync(isZero, startDate, endDate, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Scrip data for listing
        [HttpGet("GetAllScripData")]
        public async Task<IActionResult> GetAllScripData(string? exchange, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var scripData = await _stockService.GetAllScripDataAsync(exchange, search, sortingParams);
                return Ok(scripData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Sherkhan All Trade File[.csv]
        [HttpPost("ImportSherkhanAllTradeFile")]
        public async Task<IActionResult> ImportSherkhanAllTradeFile(IFormFile formFile, string firmName, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportSherkhanTradeFileAsync(formFile, firmName, 0, overrideData);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Sherkhan Individual Trade File[.csv]
        [HttpPost("ImportSherkhanIndividualTradeFile")]
        public async Task<IActionResult> ImportSherkhanIndividualTradeFile(IFormFile formFile, string firmName, int id, bool overrideData = false)
        {
            try
            {
                var res = await _stockService.ImportSherkhanTradeFileAsync(formFile, firmName, id, overrideData);
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

        #region Import Daily Stock Price File
        [HttpPost("ImportDailyStockPriceFile")]
        public async Task<IActionResult> ImportDailyStockPriceFile(DateTime date)
        {
            try
            {
                var res = await _stockService.ImportDailyStockPriceFileAsync(date);
                return res != 0 ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Sherkhan All Client trade file.
        [HttpPost("ImportSherkhanAllClientFile")]
        public async Task<ActionResult> ImportSherkhanAllClientFile(IFormFile formFile, bool overrideData = false)
        {
            try
            {
                var flag = await _stockService.ImportAllClientSherkhanFileAsync(formFile, overrideData);
                return flag.Item1 > 0 ? Ok(new { Message = flag.Item2 }) : BadRequest(new { Message = flag.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import NSE FNO trade file.
        [HttpPost("ImportNSEFNOTradeFile")]
        public async Task<ActionResult> ImportNSEFNOTradeFile(IFormFile formFile, bool overrideData = false)
        {
            try
            {
                var flag = await _stockService.ImportNSEFNOTradeFileAsync(formFile, overrideData);
                return flag.Item1 != 0 ? Ok(new { Message = flag.Item2 }) : BadRequest(new { Message = flag.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
