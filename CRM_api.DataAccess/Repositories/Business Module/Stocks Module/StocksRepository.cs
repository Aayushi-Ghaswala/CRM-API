using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.DataAccess.ResponseModel.Stocks_Module;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CRM_api.DataAccess.Repositories.Business_Module.Stocks_Module
{
    public class StocksRepository : IStocksRepository
    {
        private readonly CRMDbContext _context;

        public StocksRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Stock data by user name 
        public async Task<int> GetStockMonthlyByUserName(string userName, DateTime date)
        {
            var stockCount = await _context.TblStockData.Where(x => x.StClientname == userName && x.StDate.Value.Month == date.Month && x.StDate.Value.Year == date.Year).CountAsync();

            return stockCount;
        }
        #endregion

        #region Get stock user's names
        public async Task<Response<UserNameResponse>> GetStocksUsersName(string? scriptName, string? firmName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var userNameResponse = new List<UserNameResponse>();
            var filterData = userNameResponse.AsQueryable();

            if (searchingParams != null)
                filterData = _context.Search<TblStockData>(searchingParams).Where(s => (string.IsNullOrEmpty(scriptName) || (!string.IsNullOrEmpty(scriptName) && s.StScripname.ToLower().Equals(scriptName.ToLower()))) && (string.IsNullOrEmpty(firmName) || (!string.IsNullOrEmpty(firmName) && s.FirmName.ToLower().Equals(firmName.ToLower())))).Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();

            else
                filterData = _context.TblStockData.Where(s => (string.IsNullOrEmpty(scriptName) || (!string.IsNullOrEmpty(scriptName) && s.StScripname.ToLower().Equals(scriptName.ToLower()))) && (string.IsNullOrEmpty(firmName) || (!string.IsNullOrEmpty(firmName) && s.FirmName.ToLower().Equals(firmName.ToLower())))).Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stockData = new Response<UserNameResponse>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };
            return stockData;
        }
        #endregion

        #region Get all/client wise script names
        public async Task<Response<ScriptNameResponse>> GetAllScriptNames(string clientName, string? firmName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<ScriptNameResponse?> stockDataList = null;
            
            if (searchingParams != null)
                stockDataList = _context.Search<TblStockData>(searchingParams).Where(s => (string.IsNullOrEmpty(clientName) || (!string.IsNullOrEmpty(s.StClientname) && s.StClientname.ToLower().Equals(clientName.ToLower()))) && (string.IsNullOrEmpty(firmName) || (!string.IsNullOrEmpty(firmName) && s.FirmName.ToLower().Equals(firmName.ToLower())))).Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();

            else
                stockDataList = _context.TblStockData.Where(s => (string.IsNullOrEmpty(clientName) || ((!string.IsNullOrEmpty(s.StClientname) && s.StClientname.ToLower().Equals(clientName.ToLower())))) && (string.IsNullOrEmpty(firmName) || ((!string.IsNullOrEmpty(firmName) && s.FirmName.ToLower().Equals(firmName.ToLower()))))).Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();

            pageCount = Math.Ceiling((stockDataList.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(stockDataList, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stockData = new Response<ScriptNameResponse>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };
            return stockData;
        }
        #endregion

        #region Get All Scrip
        public async Task<List<TblScripMaster>> GetAllScrip()
        {
            return await _context.TblScripMasters.ToListAsync();
        }
        #endregion

        #region Get All Scrip data for listing
        public async Task<Response<TblScripMaster>> GetAllScripData(string? exchange, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<TblScripMaster?> scripList = null;

            if (search != null)
                scripList = _context.Search<TblScripMaster>(search).Where(s => (string.IsNullOrEmpty(exchange) || s.Exchange.ToLower().Equals(exchange.ToLower()))).AsQueryable();
            else
                scripList = _context.TblScripMasters.Where(s => (string.IsNullOrEmpty(exchange) || s.Exchange.ToLower().Equals(exchange.ToLower()))).AsQueryable();

            pageCount = Math.Ceiling((scripList.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(scripList, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var scripData = new Response<TblScripMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };
            return scripData;
        }
        #endregion

        #region Calculate intraDay/Delivery Buy/Sale
        public async Task<(decimal?, decimal?, decimal?, decimal?, decimal?, decimal?)> CalculateIntradayDeliveryAmount(IQueryable<TblStockData> filterData, List<TblScripMaster> scrips)
        {
            decimal? totalIntraBuyAmount = 0.0m;
            decimal? totalIntraSaleAmount = 0.0m;
            decimal? totalDeliveryBuyAmount = 0.0m;
            decimal? totalDeliverySaleAmount = 0.0m;

            var stockDataLookup = filterData.ToLookup(s => s.StClientname);

            foreach (var clientName in stockDataLookup)
            {
                var clientWiseStock = clientName.ToList();

                var scripWiseStockLookup = clientWiseStock.ToLookup(s => s.StScripname);

                foreach (var scripName in scripWiseStockLookup)
                {
                    var scripWiseStock = scripName.ToList();

                    var dateWiseStockLookup = scripWiseStock.ToLookup(s => s.StDate);

                    decimal? price = 0.0m;
                    var scrip = scrips.FirstOrDefault(x => x.Scripname != null && x.Scripname.ToLower() == scripName.Key.ToLower());

                    if (scrip is not null)
                        price = scrip.Ltp;
                    else
                        price = Math.Round((decimal)scripWiseStock.Last().StNetsharerate, 2);

                    foreach (var stock in dateWiseStockLookup)
                    {
                        var totalBuyQty = stock.Where(s => s.StType == "B").Sum(s => s.StQty);
                        var totalSaleQty = stock.Where(s => s.StType == "S").Sum(s => s.StQty);

                        if (totalBuyQty == totalSaleQty)
                        {
                            totalIntraSaleAmount += stock.Where(s => s.StType == "S").Sum(s => s.StNetcostvalue);
                            totalIntraBuyAmount += (int)stock.Where(s => s.StType == "B").Sum(s => s.StNetcostvalue);
                        }
                        else if (totalBuyQty > totalSaleQty)
                        {
                            totalIntraSaleAmount += stock.Where(s => s.StType == "S").Sum(s => s.StNetcostvalue);
                            var rate = stock.Where(s => s.StType == "B").FirstOrDefault().StNetsharerate;
                            totalIntraBuyAmount += rate * totalSaleQty;
                            totalDeliveryBuyAmount += rate * (totalBuyQty - totalSaleQty);
                        }
                        else
                        {
                            totalDeliverySaleAmount += price * (totalSaleQty - totalBuyQty);
                        }
                    }
                }
            }

            var totalPurchase = filterData.Where(s => s.StType.Equals("B")).Sum(x => x.StNetcostvalue);
            var totalSale = filterData.Where(s => s.StType.Equals("S")).Sum(x => x.StNetcostvalue);

            return (totalIntraBuyAmount, totalIntraSaleAmount, totalDeliveryBuyAmount, totalDeliverySaleAmount, totalPurchase, totalSale);
        }
        #endregion

        #region Get stocks transaction data
        public async Task<StocksResponse<TblStockData>> GetStocksTransactions(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? fileType, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblStockData> data = new List<TblStockData>();
            IQueryable<TblStockData> filterData = data.AsQueryable();

            if (searchingParams != null)
                filterData = _context.Search<TblStockData>(searchingParams).Where(s => (string.IsNullOrEmpty(clientName) || (!string.IsNullOrEmpty(clientName) && s.StClientname.ToLower().Equals(clientName.ToLower()))) && (fromDate == null || (s.StDate != null && s.StDate >= fromDate)) && (toDate == null || (s.StDate != null && s.StDate <= toDate)) && (string.IsNullOrEmpty(scriptName) || (!string.IsNullOrEmpty(scriptName) && s.StScripname.ToLower().Equals(scriptName.ToLower()))) && (string.IsNullOrEmpty(firmName) || (!string.IsNullOrEmpty(s.FirmName) && s.FirmName.ToLower().Equals(firmName.ToLower()))) && (string.IsNullOrEmpty(fileType) || (!string.IsNullOrEmpty(s.FileType) && s.FileType.ToLower().Equals(fileType.ToLower())))).AsQueryable();
            else
                filterData = _context.TblStockData.Where(s => (string.IsNullOrEmpty(clientName) || (!string.IsNullOrEmpty(clientName) && s.StClientname.ToLower().Equals(clientName.ToLower()))) && (fromDate == null || (s.StDate != null && s.StDate >= fromDate)) && (toDate == null || (s.StDate != null && s.StDate <= toDate)) && (string.IsNullOrEmpty(scriptName) || (!string.IsNullOrEmpty(scriptName) && s.StScripname.ToLower().Equals(scriptName.ToLower()))) && (string.IsNullOrEmpty(firmName) || (!string.IsNullOrEmpty(firmName) && s.FirmName.ToLower().Equals(firmName.ToLower()))) && (string.IsNullOrEmpty(fileType) || (!string.IsNullOrEmpty(s.FileType) && s.FileType.ToLower().Equals(fileType.ToLower())))).AsQueryable();

            var scrips = await GetAllScrip();
            var BuySaleReport = await CalculateIntradayDeliveryAmount(filterData, scrips);

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stockData = new Response<TblStockData>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };
            var stockResponse = new StocksResponse<TblStockData>()
            {
                response = stockData,
                TotalIntradayBuy = BuySaleReport.Item1,
                TotalIntradaySale = BuySaleReport.Item2,
                TotalDeliveryBuy = BuySaleReport.Item3,
                TotalDeliverySale = BuySaleReport.Item4,
                TotalPurchase = BuySaleReport.Item5,
                TotalSale = BuySaleReport.Item6,
            };
            return stockResponse;
        }
        #endregion

        #region Get stocks data for specific date range
        public async Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate)
        {
            List<TblStockData> stockDatas = new List<TblStockData>();

            stockDatas = await _context.TblStockData.Where(s => (startDate == null || (s.StDate != null && s.StDate >= startDate)) && (endDate == null || (s.StDate != null && s.StDate <= endDate))).ToListAsync();

            return stockDatas;
        }
        #endregion

        #region Get stocks data from specific date range for import
        public async Task<List<TblStockData>> GetStockDataFromSpecificDateRangeForImport(DateTime? startDate, DateTime? endDate, string firmName, string fileType)
        {
            List<TblStockData> stockDatas = new List<TblStockData>();

            stockDatas = await _context.TblStockData.Where(s => s.StDate >= startDate && s.StDate <= endDate && s.FileType.ToLower().Equals(fileType.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).ToListAsync();

            return stockDatas;
        }
        #endregion

        #region Get stocks transaction current stock amount by user name
        public async Task<List<TblStockData>> GetStockDataByUserName(string userName, DateTime? startDate, DateTime? endDate)
        {
            List<TblStockData> stockData = new List<TblStockData>();

            stockData = await _context.TblStockData.Where(s => (startDate == null || (s.StDate != null && s.StDate >= startDate)) && (endDate == null || (s.StDate != null && s.StDate <= endDate)) && (string.IsNullOrEmpty(userName) || (!string.IsNullOrEmpty(s.StClientname) && s.StClientname.ToLower().Equals(userName)))).ToListAsync();

            return stockData;
        }
        #endregion

        #region Get Stock Details By UserNames
        public async Task<List<TblStockData>> RetrieveStocksByUsernames(IEnumerable<string> usernames)
        {
            List<TblStockData> stocksData = await _context.TblStockData.Where(x => usernames.Contains(x.StClientname.ToLower())).ToListAsync();

            return stocksData;
        }
        #endregion

        #region Get monthly stock transaction by user name
        public async Task<List<TblStockData>> GetCurrentStockDataByUserName(int? month, int? year, IEnumerable<string> userNames)
        {
            List<TblStockData> stockData = new List<TblStockData>();

            if (month is not null && year is not null)
                stockData = await _context.TblStockData.Where(x => userNames.Contains(x.StClientname.ToLower()) && x.StDate.Value.Month == month && x.StDate.Value.Year == year && x.StType.Equals("B") && x.StType.Equals("S")).ToListAsync();
            else
                stockData = await _context.TblStockData.Where(x => userNames.Contains(x.StClientname.ToLower()) && x.StDate.Value.Month == DateTime.Now.Month && x.StDate.Value.Year == DateTime.Now.Year && x.StType.Equals("B") && x.StType.Equals("S")).ToListAsync();

            return stockData;
        }
        #endregion

        #region Add stocks data
        public async Task<int> AddData(List<TblStockData> tblStockData)
        {
            await _context.TblStockData.AddRangeAsync(tblStockData);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Scrip Data
        public async Task<int> UpdateScripData(List<TblScripMaster> scripMasters)
        {
            _context.TblScripMasters.UpdateRange(scripMasters);
            return await _context.SaveChangesAsync();
        }
        #endregion 

        #region Delete stocks data
        public Task<int> DeleteData(List<TblStockData> tblStockData)
        {
            _context.TblStockData.RemoveRange(tblStockData);
            return _context.SaveChangesAsync();
        }
        #endregion
    }
}
