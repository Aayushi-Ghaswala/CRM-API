using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.MutualFunds_Module
{
    public class MutualfundRepositery : IMutualfundRepositry
    {
        private readonly CRMDbContext _context;

        public MutualfundRepositery(CRMDbContext context)
        {
            _context = context;
        }

        #region Add Mutual Fund Details To Exist User Table
        public async Task<int> AddMFDataForExistUser(List<TblMftransaction> tblMftransaction)
        {
            await _context.TblMftransactions.AddRangeAsync(tblMftransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Mutual Fund Details To Not Exist User Table
        public async Task<int> AddMFDataForNotExistUser(List<TblNotexistuserMftransaction> tblNotexistuserMftransaction)
        {
            await _context.TblNotexistuserMftransactions.AddRangeAsync(tblNotexistuserMftransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Mutual Funds Record in Specific Date
        public async Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? StartDate, DateTime? EndDate)
        {
            var getData = await _context.TblMftransactions.Where(x => x.Date >= StartDate && x.Date <= EndDate).ToListAsync();
            return getData;
        }
        #endregion

        #region Get Mutual Funds Record in Specific Date For Not Exist User 
        public async Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? StartDate, DateTime? EndDate)
        {
            var getData = await _context.TblNotexistuserMftransactions.Where(x => x.Date >= StartDate && x.Date <= EndDate).ToListAsync();
            return getData;
        }
        #endregion

        #region Delete Client Wise Mutualfund Transaction In User Exist Table
        public async Task<int> DeleteMFForUserExist(TblMftransaction tblMftransaction)
        {
            _context.TblMftransactions.Remove(tblMftransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Client Wise Mutualfund Transaction In Not Exist User Table
        public async Task<int> DeleteMFForNotUserExist(TblNotexistuserMftransaction tblMftransaction)
        {
            _context.TblNotexistuserMftransactions.Remove(tblMftransaction);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get SchemeId by SchemeName
        public int GetSchemeIdBySchemeName(string schemeName)
        {
            var scheme = _context.TblMfSchemeMasters.Where(x => x.SchemeName == schemeName).FirstOrDefault();

            if (scheme == null)
                return 0;
            return scheme.SchemeId;
        }
        #endregion
    }
}
