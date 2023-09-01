using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;

namespace CRM_api.Services.Services.Business_Module.Stocks_Module
{
    public class StocksDashboardService : IStocksDashboardService
    {
        private readonly IStocksDashboardRepository _stocksDashboardRepository;
        private readonly IStocksRepository _stocksRepository;

        public StocksDashboardService(IStocksDashboardRepository stocksDashboardRepository, IStocksRepository stocksRepository)
        {
            _stocksDashboardRepository = stocksDashboardRepository;
            _stocksRepository = stocksRepository;
        }

        #region get stock summary report
        public async Task<List<StocksDashboardSummaryDto>> GetStocksSummaryReportAsync()
        {
            List<StocksDashboardSummaryDto> dashboardSummaryDtos = new List<StocksDashboardSummaryDto>();
            var scrips = await _stocksRepository.GetAllScrip();
            var stockDataList = await _stocksDashboardRepository.GetAllStockData();

            // Calculate date ranges
            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfQuarter = new DateTime(today.Year, (today.Month - 1) / 3 * 3 + 1, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            // Filter stock data for different durations
            var yearDataList = stockDataList.Where(s => s.StDate.Value.Year == today.Year).ToList();
            var quarterDataList = yearDataList.Where(s => s.StDate.Value.Date >= startOfQuarter.Date && s.StDate.Value <= startOfQuarter.AddMonths(2)).ToList();
            var monthDataList = quarterDataList.Where(s => s.StDate.Value.Month == startOfMonth.Month).ToList();
            var weekDataList = monthDataList.Where(s => s.StDate.Value.Date >= startOfWeek.Date && s.StDate.Value.Date < startOfWeek.AddDays(7).Date).ToList();
            var todayDataList = weekDataList.Where(s => s.StDate.Value.Date == today.Date).ToList();

            // Calculate summaries
            dashboardSummaryDtos.Add(CalculateStockSummary("Today", todayDataList, scrips));
            dashboardSummaryDtos.Add(CalculateStockSummary("This Week", weekDataList, scrips));
            dashboardSummaryDtos.Add(CalculateStockSummary("This Month", monthDataList, scrips));
            dashboardSummaryDtos.Add(CalculateStockSummary("This Quarter", quarterDataList, scrips));
            dashboardSummaryDtos.Add(CalculateStockSummary("This Year", yearDataList, scrips));
            dashboardSummaryDtos.Add(CalculateStockSummary("All Time", stockDataList, scrips));

            //dashboardSummaryDtos.Reverse();
            return dashboardSummaryDtos;
        }
        #endregion

        #region stock summary calculation
        private StocksDashboardSummaryDto CalculateStockSummary(string duration, List<TblStockData> stockData, List<TblScripMaster> scrips)
        {
            var stockList = stockData.GroupBy(s => s.StClientname).ToList();
            decimal? totalIntraAmount = 0.0m;
            decimal? totalDeliveryAmount = 0.0m;
            int totalDeliveryClient = 0;
            int totalIntraClient = 0;

            foreach (var clientWiseStock in stockList)
            {
                decimal? intraAmount = 0;
                decimal? deliveryAmount = 0;
                var scripWiseStockList = clientWiseStock.GroupBy(s => s.StScripname).ToList();
                foreach (var scripWiseStock in scripWiseStockList)
                {
                    var dateWiseStockList = scripWiseStock.GroupBy(s => s.StDate).ToList();
                    decimal? price = 0.0m;
                    var scrip = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Equals(scripWiseStock.Key.ToLower())).FirstOrDefault();
                    if (scrip is not null)
                        price = scrip.Ltp;
                    else
                        price = Math.Round((decimal)scripWiseStock.Last().StNetsharerate, 2);
                    foreach (var stock in dateWiseStockList)
                    {
                        var totalBuyQty = stock.Where(s => s.StType == "B").Sum(s => s.StQty);
                        var totalSaleQty = stock.Where(s => s.StType == "S").Sum(s => s.StQty);

                        if (totalBuyQty == totalSaleQty)
                        {
                            intraAmount += stock.Sum(s => s.StNetcostvalue);
                        }
                        else if (totalBuyQty > totalSaleQty)
                        {
                            intraAmount += stock.Where(s => s.StType == "S").Sum(s => s.StNetcostvalue);
                            deliveryAmount += price * (totalBuyQty - totalSaleQty);
                        }
                    }
                }
                if (intraAmount > 0)
                {
                    totalIntraAmount += intraAmount;
                    totalIntraClient += 1;
                }
                if (deliveryAmount > 0)
                {
                    totalDeliveryAmount += deliveryAmount;
                    totalDeliveryClient += 1;
                }
            }
            var totalClients = stockList.Count();
            decimal totalClientAmount = Math.Round((decimal)stockData.Where(s => s.StType == "B").Sum(s => s.StNetcostvalue), 2);
            return new StocksDashboardSummaryDto(duration, totalClients, totalClientAmount, totalDeliveryClient, totalDeliveryAmount, totalIntraClient, totalIntraAmount);
        }
        #endregion

        #region get summary chart report
        public async Task<List<HoldingChartReportDto>> GetSummaryChartReportAsync(DateTime fromDate, DateTime toDate)
        {
            var stockData = await _stocksDashboardRepository.GetStockDataOfDateRange(fromDate, toDate);
            var monthDiffrence = (12 * (toDate.Year - fromDate.Year) + toDate.Month - fromDate.Month) + 1;
            var scrips = await _stocksRepository.GetAllScrip();
            List<HoldingChartReportDto> holdingChartReportDtos = new List<HoldingChartReportDto>();

            for (int i = 0; i < monthDiffrence; i++)
            {
                var holdingChartDto = new HoldingChartReportDto();
                var date = fromDate.AddMonths(i);
                decimal? currentAmount = 0;

                holdingChartDto.Month = date.ToString("MMM-yyyy");
                var currentMonthData = stockData.Where(x => x.StDate.Value.Month == date.Month && x.StDate.Value.Year == date.Year).ToList();
                holdingChartDto.UserCount = currentMonthData.DistinctBy(x => x.StScripname).Count();

                var scripWiseData = currentMonthData.GroupBy(s => s.StScripname).ToList();
                foreach (var stock in scripWiseData)
                {
                    var totalBuyQty = stock.Where(s => s.StType == "B").Sum(s => s.StQty);
                    var totalSaleQty = stock.Where(s => s.StType == "S").Sum(s => s.StQty);
                    var availableQty = totalBuyQty - totalSaleQty;

                    decimal? price = 0.0m;
                    var scrip = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Equals(stock.Key.ToLower())).FirstOrDefault();
                    if (scrip is not null)
                        price = scrip.Ltp;
                    else
                        price = Math.Round((decimal)stock.Last().StNetsharerate, 2);

                    holdingChartDto.CurrentValue = Math.Round((decimal)availableQty * (decimal)price, 2);
                }
                holdingChartReportDtos.Add(holdingChartDto);
            }
            return holdingChartReportDtos;
        }
        #endregion
    }
}
