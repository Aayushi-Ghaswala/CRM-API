using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface IStockService
    {
        Task<int> ImportSharekhanTradeFile(IFormFile formFile, string firmName, int id, bool overrideData);
        Task<int> ImportJainamTradeFile(IFormFile formFile, string firmName, string clientName, bool overrideData);
    }
}
