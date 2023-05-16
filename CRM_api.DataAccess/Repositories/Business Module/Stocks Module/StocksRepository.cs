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

        public async Task<int> AddData(List<TblStockData> tblStockData)
        {
            await _context.TblStockData.AddRangeAsync(tblStockData);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate)
        {
            return await _context.TblStockData.Where(s => s.StDate >= startDate && s.StDate <= endDate).ToListAsync();
        }

        public Task<int> UpdateData(List<TblStockData> tblStockData)
        {
            _context.ChangeTracker.Clear();
            _context.TblStockData.UpdateRange(tblStockData);
            return _context.SaveChangesAsync();
        }
    }
}
