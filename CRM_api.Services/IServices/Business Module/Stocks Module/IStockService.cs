using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStockService
    {
        Task<ResponseDto<UserNameDto>> GetStocksUsersNameAsync(string? scriptName, string? firmName, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<ScriptNamesDto>> GetAllScriptNamesAsync(string clientName, string? firmName, string? searchingParams, SortingParams sortingParams);
        Task<StockResponseDto<StockMasterDto>> GetStockDataAsync(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? searchingParams, SortingParams sortingParams);
        Task<StockSummaryDto<ScripwiseSummaryDto>> GetClientwiseScripSummaryAsync(string? userName, bool? isZero, DateTime? startDate, DateTime? endDate, string? searchingParams, SortingParams sortingParams);
        Task<StockSummaryDto<ScripwiseSummaryDto>> GetAllClientwiseStockSummaryAsync(bool? isZero, DateTime? startDate, DateTime? endDate, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportSherkhanTradeFileAsync(IFormFile formFile, string firmName, int id, bool overrideData);
        Task<int> ImportJainamTradeFileAsync(IFormFile formFile, string firmName, string clientName, bool overrideData);
        Task<int> ImportDailyStockPriceFileAsync(DateTime date);
        Task<int> ImportAllClientSherkhanFileAsync(IFormFile formFile, bool overrideData);
        Task<(int, string)> ImportNSEFNOTradeFileAsync(IFormFile formFile, bool overrideData);
    }
}
