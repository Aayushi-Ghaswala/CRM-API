using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.Stocks_Module
{
    public interface ISharekhanStockService
    {
        Task<int> ImportTradeFile(IFormFile formFile, int id, bool overrideData);
        //Task<int> ImportALLTradeFile(string filePath);
    }
}
