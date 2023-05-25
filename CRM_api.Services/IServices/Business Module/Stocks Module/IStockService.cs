using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStockService
    {
        Task<StockResponseDto<StockMasterDto>> GetStockData(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportSharekhanTradeFile(IFormFile formFile, string firmName, int id, bool overrideData);
        Task<int> ImportJainamTradeFile(IFormFile formFile, string firmName, string clientName, bool overrideData);
    }
}
