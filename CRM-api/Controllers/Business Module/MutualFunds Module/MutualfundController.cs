﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.MutualFunds_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutualfundController : ControllerBase
    {
        private readonly IMutualfundService _mutualfundService;

        public MutualfundController(IMutualfundService mutualfundService)
        {
            _mutualfundService = mutualfundService;
        }

        [HttpGet("MFClientWiseTransaction")]
        #region Display Mutual Fund By UserId and SchemeName
        public async Task<IActionResult> GetUserWiseMFTransaction(int userId, int? schemeId
                                            , [FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams, DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var mfDetails = await _mutualfundService.GetClientwiseMutualFundTransactionAsync(userId, schemeId, searchingParams, sortingParams, StartDate, EndDate);
                return Ok(mfDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("MFClientSchemeWiseSummary")]
        #region Get client wise MF Fund wise Summary
        public async Task<IActionResult> GetUserSchemeWiseMFSummary(int userId, [FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetMFSummaryAsync(userId, searchingParams, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("MFSummaryCategoryWise")]
        #region Get MF Client Wise Category Wise Summary
        public async Task<IActionResult> GetUserCategoryWiseMFSummary(int userId, [FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetMFCategoryWiseAsync(userId, searchingParams, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("AllClientMFSummary")]
        #region Get All Client MF Summary
        public async Task<IActionResult> GetAllClientMFSummary(DateTime FromDate, DateTime ToDate, [FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetAllClientMFSummaryAsync(FromDate, ToDate, searchingParams, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet("GetMFUserName")]
        #region Get Mf UserName
        public async Task<IActionResult> GetMfUserName([FromQuery] string? searchingParams,[FromQuery] SortingParams sortingParams)
        {
            var mfUser = await _mutualfundService.GetMFUserNameAsync(searchingParams, sortingParams);
            return Ok(mfUser);
        }
        #endregion

        [HttpGet("SchemaName")]
        #region Display Scheme Name by UserId
        public async Task<IActionResult> GetSchemeName(int userId, [FromQuery] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var schemeName = await _mutualfundService.DisplayschemeNameAsync(userId, searchingParams, sortingParams);
                return Ok(schemeName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost("ImportNJCLientFile")]
        #region Import NJ Client File
        public async Task<IActionResult> ImportNJClientExcel(IFormFile file, bool UpdateIfExist)
        {
            try
            {
                var flag = await _mutualfundService.ImportNJClientFileAsync(file, UpdateIfExist);
                return (flag == 0) ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
