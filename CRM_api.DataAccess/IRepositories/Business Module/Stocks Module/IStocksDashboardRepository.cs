using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Stocks_Module;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module
{
    public interface IStocksDashboardRepository
    {
        Task<List<vw_StockData>> GetStockDataOfDateRange(DateTime toDate);
        Task<List<vw_StockData>> GetAllStockData();
        Task<List<StocksDashboardIntraDeliveryResponse>> GetIntraDeliveryReport();
    }
}
