using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStockService
    {
        Task<ResponseDto<StocksClientNamesDto>> GetStocksUsersNameAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<ScriptNamesDto>> GetAllScriptNamesAsync(string clientName, string? searchingParams, SortingParams sortingParams);
        Task<StockResponseDto<StockMasterDto>> GetStockDataAsync(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportSharekhanTradeFileAsync(IFormFile formFile, string firmName, int id, bool overrideData);
        Task<int> ImportJainamTradeFileAsync(IFormFile formFile, string firmName, string clientName, bool overrideData);
    }
}
