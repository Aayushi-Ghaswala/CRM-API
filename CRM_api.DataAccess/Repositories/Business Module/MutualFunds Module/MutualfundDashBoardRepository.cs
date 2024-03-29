﻿using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.MutualFunds_Module
{
    public class MutualfundDashBoardRepository : IMutualfundDashBoardRepository
    {
        private readonly CRMDbContext _context;

        public MutualfundDashBoardRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Top 10 Scheme Investmentwise
        public async Task<List<GetTopTenSchemeByInvestment>> GetTopTenSchemeByInvestmentWise()
        {
            var mfTopTenScheme = _context.GetTopTenSchemeByInvestments.FromSqlRaw("EXECUTE kaadmin.GetTopTenSchemeByInvestment").ToList();
            return mfTopTenScheme;
        }
        #endregion

        #region Get Mutual Funds Record in Specific Date
        public async Task<List<vw_MFChartHolding>> GetMFInSpecificDateForExistUser(DateTime? endDate)
        {
            var getData = await _context.Vw_MFChartHoldings.Where(x => x.Date <= endDate).ToListAsync();
            return getData;
        }
        #endregion

        #region Get All MF Transaction
        public async Task<List<vw_Mftransaction>> GetAllMFTransaction()
        {
            return _context.Vw_Mftransactions.ToList();
        }
        #endregion
    }
}
