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

        #region Get stock user's names
        public async Task<Response<UserNameResponse>> GetStocksUsersName(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblStockData.Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();

            if (searchingParams != null)
            {
                filterData = filterData.Where(x => x.UserName.ToLower().Contains(searchingParams.ToLower()));
            }

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
        public async Task<Response<TblStockData>> GetAllScriptNames(string clientName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblStockData> data = new List<TblStockData>();
            List<TblStockData> stockDataList = new List<TblStockData>();
            IQueryable<TblStockData> filterData = data.AsQueryable();
            if (!string.IsNullOrEmpty(clientName))
                stockDataList = await _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower())).ToListAsync();
            else
                stockDataList = await _context.TblStockData.ToListAsync();

            filterData = stockDataList.DistinctBy(s => s.StScripname).AsQueryable();

            if (searchingParams != null)
            {
                if (!string.IsNullOrEmpty(clientName))
                    data = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower())).ToList();
                else
                    data = _context.Search<TblStockData>(searchingParams).ToList();
                filterData = data.DistinctBy(s => s.StScripname).AsQueryable();
            }
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
            return stockData;
        }
        #endregion

        #region Get stocks transaction data
        public async Task<StocksResponse<TblStockData>> GetStocksTransactions(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblStockData> data = new List<TblStockData>();
            IQueryable<TblStockData> filterData = data.AsQueryable();

            decimal? totalPurchase = 0;
            //decimal? totalPurchaseQty = 0;
            decimal? totalSale = 0;
            //decimal? totalSaleQty = 0;
            if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(scriptName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(firmName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(firmName) || !string.IsNullOrWhiteSpace(firmName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(clientName) || !string.IsNullOrWhiteSpace(clientName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower())).AsQueryable();
            }
            else if (!string.IsNullOrEmpty(scriptName) || !string.IsNullOrWhiteSpace(scriptName))
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower())).AsQueryable();
            }
            else
            {
                if (fromDate != null && toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                else if (fromDate != null)
                    filterData = _context.TblStockData.Where(s => s.StDate >= fromDate).AsQueryable();
                else if (toDate != null)
                    filterData = _context.TblStockData.Where(s => s.StDate <= toDate).AsQueryable();
                else
                    filterData = _context.TblStockData.AsQueryable();
            }

            if (searchingParams != null)
            {
                if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(scriptName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate);
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate);
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate <= toDate);
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StScripname.ToLower().Equals(scriptName.ToLower()));
                }
                else if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(firmName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(firmName) || !string.IsNullOrWhiteSpace(firmName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.FirmName.ToLower().Equals(firmName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.FirmName.ToLower().Equals(firmName.ToLower())).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(clientName) || !string.IsNullOrWhiteSpace(clientName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower())).AsQueryable();
                }
                else if (!string.IsNullOrEmpty(scriptName) || !string.IsNullOrWhiteSpace(scriptName))
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower()) && s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StScripname.ToLower().Equals(scriptName.ToLower())).AsQueryable();
                }
                else
                {
                    if (fromDate != null && toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StDate >= fromDate && s.StDate <= toDate).AsQueryable();
                    else if (fromDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StDate >= fromDate).AsQueryable();
                    else if (toDate != null)
                        filterData = _context.Search<TblStockData>(searchingParams).Where(s => s.StDate <= toDate).AsQueryable();
                    else
                        filterData = _context.Search<TblStockData>(searchingParams).AsQueryable();
                }
                //filterData = _context.Search<TblStockData>(searchingParams).Where(s => );
                //filterData = _context.Search<TblStockData>(searchingParams);
            }
            totalPurchase = filterData.Where(s => s.StType.Equals("B")).Sum(x => x.StNetcostvalue);
            //totalPurchaseQty = filterData.Where(s => s.StType.Equals("B")).Sum(x => x.StQty);
            totalSale = filterData.Where(s => s.StType.Equals("S")).Sum(x => x.StNetcostvalue);
            //totalSaleQty = filterData.Where(s => s.StType.Equals("S")).Sum(x => x.StQty);
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
                TotalPurchase = totalPurchase,
                //TotalPurchaseQty = totalPurchaseQty,
                TotalSale = totalSale,
                //TotalSaleQty = totalSaleQty
            };
            return stockResponse;
        }
        #endregion

        #region Get stocks data for specific date range
        public async Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate, string firmName)
        {
            return await _context.TblStockData.Where(s => s.StDate >= startDate && s.StDate <= endDate && s.FirmName.ToLower().Equals(firmName.ToLower())).ToListAsync();
        }
        #endregion

        #region Add stocks data
        public async Task<int> AddData(List<TblStockData> tblStockData)
        {
            await _context.TblStockData.AddRangeAsync(tblStockData);
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
