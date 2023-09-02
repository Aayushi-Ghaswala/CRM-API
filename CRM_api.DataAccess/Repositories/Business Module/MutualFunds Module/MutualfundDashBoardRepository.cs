using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public async Task<IQueryable<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate)
        {
            var getData = _context.TblMftransactions.Where(x => x.Date >= startDate && x.Date <= endDate).AsQueryable();
            return getData;
        }
        #endregion

        #region Get All MF Transaction
        public async Task<List<TblMftransaction>> GetAllMFTransaction()
        {
            return _context.TblMftransactions.ToList();
        }
        #endregion
    }
}
