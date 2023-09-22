using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Stocks_Module;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Stocks_Module
{
    public class StocksDashboardRepository : IStocksDashboardRepository
    {
        private readonly CRMDbContext _context;
        private readonly IStocksRepository _stocksRepository;

        public StocksDashboardRepository(CRMDbContext context, IStocksRepository stocksRepository)
        {
            _context = context;
            _stocksRepository = stocksRepository;
        }

        #region get stock data from date range
        public async Task<List<vw_StockData>> GetStockDataOfDateRange(DateTime toDate)
        {
            return await _context.Vw_StockDatas.Where(s => s.StDate <= toDate).ToListAsync();
        }
        #endregion

        #region get all stock transactions
        public async Task<List<vw_StockData>> GetAllStockData()
        {
            return await _context.Vw_StockDatas.ToListAsync();
        }
        #endregion

        #region get intraday delivery report
        public async Task<List<StocksDashboardIntraDeliveryResponse>> GetIntraDeliveryReport()
        {
            var stockDataList = _context.TblStockData.AsQueryable();
            var scrips = await _stocksRepository.GetAllScrip();
            List<StocksDashboardIntraDeliveryResponse> stocksDashboardIntraDeliveryResponses = new List<StocksDashboardIntraDeliveryResponse>();
            // Calculate date ranges
            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfQuarter = new DateTime(today.Year, (today.Month - 1) / 3 * 3 + 1, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            // Filter stock data for different durations
            var yearDataList = stockDataList.Where(s => s.StDate.Value.Year == today.Year).AsQueryable();
            var quarterDataList = yearDataList.Where(s => s.StDate.Value.Date >= startOfQuarter.Date && s.StDate.Value <= startOfQuarter.AddMonths(2)).AsQueryable();
            var monthDataList = quarterDataList.Where(s => s.StDate.Value.Month == startOfMonth.Month).AsQueryable();
            var weekDataList = monthDataList.Where(s => s.StDate.Value.Date >= startOfWeek.Date && s.StDate.Value.Date < startOfWeek.AddDays(7).Date).AsQueryable();
            var todayDataList = weekDataList.Where(s => s.StDate.Value.Date == today.Date).AsQueryable();

            var result = await _stocksRepository.CalculateIntradayDeliveryAmount(todayDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("Today", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));
            result = await _stocksRepository.CalculateIntradayDeliveryAmount(weekDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("This Week", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));
            result = await _stocksRepository.CalculateIntradayDeliveryAmount(monthDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("This Month", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));
            result = await _stocksRepository.CalculateIntradayDeliveryAmount(quarterDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("This Quarter", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));
            result = await _stocksRepository.CalculateIntradayDeliveryAmount(yearDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("This Year", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));
            result = await _stocksRepository.CalculateIntradayDeliveryAmount(stockDataList, scrips);
            stocksDashboardIntraDeliveryResponses.Add(new StocksDashboardIntraDeliveryResponse("All Time", result.Item1, result.Item2, result.Item3, result.Item4, result.Item5, result.Item6));

            return stocksDashboardIntraDeliveryResponses;
        }
        #endregion
    }
}
