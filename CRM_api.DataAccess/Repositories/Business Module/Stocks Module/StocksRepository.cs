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
            if (!string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
                filterData = _context.TblStockData.Where(c => c.StScripname.ToLower().Equals(scriptName.ToLower()) && c.FirmName.ToLower().Equals(firmName.ToLower()))
                                            .Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
            else if (!string.IsNullOrEmpty(scriptName))
                filterData = _context.TblStockData.Where(c => c.StScripname.ToLower().Equals(scriptName.ToLower())).Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
            else if (!string.IsNullOrEmpty(firmName))
                filterData = _context.TblStockData.Where(c => c.FirmName.ToLower().Equals(firmName.ToLower())).Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
            else
                filterData = _context.TblStockData.Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();

            if (searchingParams != null)
            {
                if (!string.IsNullOrEmpty(scriptName) && !string.IsNullOrEmpty(firmName))
                    filterData = _context.Search<TblStockData>(searchingParams).Where(c => c.StScripname.ToLower().Equals(scriptName.ToLower()) && c.FirmName.ToLower().Equals(firmName.ToLower()))
                                            .Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
                else if (!string.IsNullOrEmpty(scriptName))
                    filterData = _context.Search<TblStockData>(searchingParams).Where(c => c.StScripname.ToLower().Equals(scriptName.ToLower()))
                                            .Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
                else if (!string.IsNullOrEmpty(firmName))
                    filterData = _context.Search<TblStockData>(searchingParams).Where(c => c.FirmName.ToLower().Equals(firmName.ToLower()))
                                            .Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
                else
                    filterData = _context.Search<TblStockData>(searchingParams).Select(x => new UserNameResponse { UserName = x.StClientname }).Distinct().AsQueryable();
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
        public async Task<Response<ScriptNameResponse>> GetAllScriptNames(string clientName, string? firmName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<ScriptNameResponse?> stockDataList = null;
            //IQueryable<TblStockData> filterData = data.AsQueryable();
            if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(firmName))
                stockDataList = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()))
                                            .Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
            else if (!string.IsNullOrEmpty(clientName))
                stockDataList = _context.TblStockData.Where(s => s.StClientname.ToLower().Equals(clientName.ToLower())).Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
            else if (!string.IsNullOrEmpty(firmName))
                stockDataList = _context.TblStockData.Where(s => s.FirmName.ToLower().Equals(firmName.ToLower())).Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
            else
                stockDataList = _context.TblStockData.Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();

            if (searchingParams != null)
            {
                if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(firmName))
                    stockDataList = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()) && s.FirmName.ToLower().Equals(firmName.ToLower()))
                                                .Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
                else if (!string.IsNullOrEmpty(clientName))
                    stockDataList = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(clientName.ToLower()))
                                                .Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
                else if (!string.IsNullOrEmpty(firmName))
                    stockDataList = _context.Search<TblStockData>(searchingParams).Where(s => s.StClientname.ToLower().Equals(firmName.ToLower()))
                                                .Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
                else
                    stockDataList = _context.Search<TblStockData>(searchingParams).Select(x => new ScriptNameResponse { StScripname = x.StScripname }).Distinct().AsQueryable();
            }
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

        #region Get stocks transaction data
        public async Task<StocksResponse<TblStockData>> GetStocksTransactions(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblStockData> data = new List<TblStockData>();
            IQueryable<TblStockData> filterData = data.AsQueryable();

            decimal? totalPurchase = 0;
            decimal? totalSale = 0;
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

            totalPurchase = filterData.Where(s => s.StType.Equals("B")).Sum(x => x.StNetcostvalue);
            totalSale = filterData.Where(s => s.StType.Equals("S")).Sum(x => x.StNetcostvalue);

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
            var stockResponse = new StocksResponse<TblStockData>()
            {
                response = stockData,
                TotalPurchase = totalPurchase,
                TotalSale = totalSale,
            };
            return stockResponse;
        }
        #endregion

        #region Get stocks data for specific date range
        public async Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate, string firmName)
        {
            List<TblStockData> stockDatas = new List<TblStockData>();

            if (startDate != null && endDate != null && firmName != null)
                stockDatas = await _context.TblStockData.Where(s => s.StDate >= startDate && s.StDate <= endDate && s.FirmName.ToLower().Equals(firmName.ToLower())).ToListAsync();
            else if (firmName == null && startDate != null && endDate != null)
                stockDatas = await _context.TblStockData.Where(s => s.StDate >= startDate && s.StDate <= endDate).ToListAsync();
            else
                stockDatas = await _context.TblStockData.ToListAsync();

            return stockDatas;
        }
        #endregion

        #region Get stocks transaction current stock amount by user name
        public async Task<List<TblStockData>> GetStockDataByUserName(string userName, DateTime? startDate, DateTime? endDate)
        {
            List<TblStockData> stockData = new List<TblStockData>();

            if (startDate is null && endDate is null)
                stockData = await _context.TblStockData.Where(x => x.StClientname.ToLower() == userName.ToLower()).ToListAsync();
            else
                stockData = await _context.TblStockData.Where(x => x.StClientname.ToLower() == userName.ToLower() && x.StDate >= startDate && x.StDate <= endDate).ToListAsync();

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
                stockData = await _context.TblStockData.Where(x => userNames.Contains(x.StClientname.ToLower()) && x.StDate.Value.Month == month && x.StDate.Value.Year == year
                                                        && x.StType.Equals("B") && x.StType.Equals("S")).ToListAsync();
            else
                stockData = await _context.TblStockData.Where(x => userNames.Contains(x.StClientname.ToLower()) && x.StDate.Value.Month == DateTime.Now.Month && x.StDate.Value.Year == DateTime.Now.Year
                                                        && x.StType.Equals("B") && x.StType.Equals("S")).ToListAsync();

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

        #region Delete stocks data
        public Task<int> DeleteData(List<TblStockData> tblStockData)
        {
            _context.TblStockData.RemoveRange(tblStockData);
            return _context.SaveChangesAsync();
        }
        #endregion
    }
}
