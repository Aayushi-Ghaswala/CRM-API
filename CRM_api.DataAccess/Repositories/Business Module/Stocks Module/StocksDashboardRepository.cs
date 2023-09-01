using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Stocks_Module
{
    public class StocksDashboardRepository : IStocksDashboardRepository
    {
        private readonly CRMDbContext _context;

        public StocksDashboardRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region get stock data from date range
        public async Task<List<TblStockData>> GetStockDataOfDateRange(DateTime fromDate, DateTime toDate)
        {
            return await _context.TblStockData.Where(s => s.StDate >= fromDate && s.StDate <= toDate).ToListAsync();
        }
        #endregion

        #region get all stock transactions
        public async Task<List<TblStockData>> GetAllStockData()
        {
            return await _context.TblStockData.ToListAsync();
        }
        #endregion
    }
}
