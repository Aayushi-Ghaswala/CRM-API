﻿using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Stocks_Module
{
    public class StocksRepository : IStocksRepository
    {
        private readonly CRMDbContext _context;

        public StocksRepository(CRMDbContext context)
        {
            _context = context;
        }

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