using CRM_api.DataAccess.Helper;
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

        #region Display Mutual Fund By UserId and SchemeName
        [HttpGet("MFClientWiseTransaction")]
        public async Task<IActionResult> GetUserWiseMFTransaction(int userId, string? schemeName, string? folioNo
                                            , [FromQuery] string? search, [FromQuery] SortingParams? sortingParams, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var mfDetails = await _mutualfundService.GetClientwiseMutualFundTransactionAsync(userId, schemeName, folioNo, search, sortingParams, startDate, endDate);
                return Ok(mfDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get client wise MF Fund wise Summary
        [HttpGet("MFClientSchemeWiseSummary")]
        public async Task<IActionResult> GetUserSchemeWiseMFSummary(int userId, bool? isBalanceUnitZero, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetMFSummaryAsync(userId, isBalanceUnitZero, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get MF Client Wise Category Wise Summary
        [HttpGet("MFSummaryCategoryWise")]
        public async Task<IActionResult> GetUserCategoryWiseMFSummary(int userId, bool? isBalanceUnitZero, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetMFCategoryWiseAsync(userId, isBalanceUnitZero, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Client MF Summary
        [HttpGet("AllClientMFSummary")]
        public async Task<IActionResult> GetAllClientMFSummary(bool? isBalanceUnitZero, DateTime fromDate, DateTime toDate, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var getData = await _mutualfundService.GetAllClientMFSummaryAsync(isBalanceUnitZero, fromDate, toDate, search, sortingParams);
                return Ok(getData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Mf UserName
        [HttpGet("GetMFUserName")]
        public async Task<IActionResult> GetMfUserName([FromQuery] string? search, [FromQuery] SortingParams sortingParams)
        {
            var mfUser = await _mutualfundService.GetMFUserNameAsync(search, sortingParams);
            return Ok(mfUser);
        }
        #endregion

        #region Display Scheme Name by UserId
        [HttpGet("SchemaName")]
        public async Task<IActionResult> GetSchemeName(int userId, string? folioNo, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var schemeName = await _mutualfundService.DisplayschemeNameAsync(userId, folioNo, search, sortingParams);
                return Ok(schemeName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Display Folio Number by UserId
        [HttpGet("FolioNo")]
        public async Task<IActionResult> GetFolioNo(int userId, string? schemeName, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var folioNo = await _mutualfundService.DisplayFolioNoAsync(userId, schemeName, search, sortingParams);
                return Ok(folioNo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import NJ Client File
        [HttpPost("ImportNJCLientFile")]  
        public async Task<IActionResult> ImportNJClientExcel(IFormFile file, bool updateIfExist)
        {
            try
            {
                var flag = await _mutualfundService.ImportNJClientFileAsync(file, updateIfExist);
                return (flag != 0) ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import CAMS Client File
        [HttpPost("ImportCAMSClientPdf")]
        public async Task<IActionResult> ImportCAMSClientPdf(IFormFile file, [FromForm] string? password, bool UpdateIfExist)
        {
            try
            {
                var flag = await _mutualfundService.ImportCAMSFileAsync(file, password, UpdateIfExist);
                return (flag == 0) ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to open file." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import Daily NJ Price
        [HttpPost("ImportNJDailyPriceFile")]
        public async Task<IActionResult> ImportNJDailyPriceFile(IFormFile file)
        {
            try
            {
                var flag = await _mutualfundService.ImportNJDailyPriceFileAsync(file);
                return (flag != 0) ? Ok(new { Message = "File imported sucessfully." }) : BadRequest(new { Message = "Unable to import file data." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Import AMFI NAV File
        [HttpPost("ImportAMFINAVFile")]
        public async Task<IActionResult> ImportAMFINAVFile()
        {
            try
            {
                var flag = await _mutualfundService.ImportAMFINAVFileAsync();
                return (flag.Item1 != 0) ? Ok(new { Message = flag.Item2 }) : BadRequest(new { Message = flag.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
