using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module
{
    public interface IStocksRepository
    {
        Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate, string firmName);
        Task<int> AddData(List<TblStockData> tblStockData);
        Task<int> DeleteData(List<TblStockData> tblStockData);
    }
}
