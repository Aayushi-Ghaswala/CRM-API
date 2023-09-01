using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module
{
    public interface IStocksDashboardRepository
    {
        Task<List<TblStockData>> GetStockDataOfDateRange(DateTime fromDate, DateTime toDate);
        Task<List<TblStockData>> GetAllStockData();
    }
}
