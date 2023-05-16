using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module
{
    public interface IStocksRepository
    {
        Task<int> AddData(List<TblStockData> tblStockData);
        Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate);
        Task<int> UpdateData(List<TblStockData> tblStockData);
    }
}
