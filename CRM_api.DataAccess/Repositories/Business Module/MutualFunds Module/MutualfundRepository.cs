using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.MutualFunds_Module
{
    public class MutualfundRepository : IMutualfundRepository
    {
        private readonly CRMDbContext _context;

        public MutualfundRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Mutual Funds Record in Specific Date
        public async Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate)
        {
            var getData = await _context.TblMftransactions.Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();
            return getData;
        }
        #endregion

        #region Get Mutual Funds Record in Specific Date For Not Exist User 
        public async Task<List<TblNotexistuserMftransaction>> GetMFInSpecificDateForNotExistUser(DateTime? startDate, DateTime? endDate)
        {
            var getData = await _context.TblNotexistuserMftransactions.Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();
            return getData;
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

        #region Get Client Wise Mutual Fund Transaction
        public async Task<BussinessResponse<TblMftransaction>> GetTblMftransactions(int userId, int? schemeId
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate)
        {
            List<TblMftransaction> TblMftransaction = new List<TblMftransaction>();
            IQueryable<TblMftransaction> mftransactions = TblMftransaction.AsQueryable();
            double pageCount = 0;

            if (startDate == null && endDate == null)
            {
                if (schemeId == null)
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId).AsQueryable();
                else
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.SchemeId == schemeId).AsQueryable();
            }
            else if (endDate == null)
            {
                if (schemeId == null)
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.Date >= startDate).AsQueryable();
                else
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.SchemeId == schemeId && x.Date >= startDate).AsQueryable();
            }
            else if (startDate == null)
            {
                if (schemeId == null)
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.Date <= endDate).AsQueryable();
                else
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.SchemeId == schemeId && x.Date <= endDate).AsQueryable();
            }
            else
            {
                if (schemeId == null)
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.Date >= startDate && x.Date <= endDate).AsQueryable();
                else
                    mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && x.SchemeId == schemeId && x.Date >= startDate && x.Date <= endDate).AsQueryable();
            }

            var redemptionUnit = mftransactions.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
            var redemUnit = redemptionUnit.Sum(x => x.Noofunit);
            var redemAmount = redemptionUnit.Sum(x => x.Invamount);

            var TotalpurchaseUnit = mftransactions.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
            var purchaseUnit = TotalpurchaseUnit.Sum(x => x.Noofunit);
            var purchaseAmount = TotalpurchaseUnit.Sum(x => x.Invamount);

            decimal? totalPurchaseunit = purchaseUnit - redemUnit;
            decimal? totalAmount = purchaseAmount - redemAmount;

            if (searchingParams != null)
                mftransactions = _context.Search<TblMftransaction>(searchingParams);

            pageCount = Math.Ceiling(mftransactions.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(mftransactions, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mutualfundData = new Response<TblMftransaction>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var mutualfundResponse = new BussinessResponse<TblMftransaction>()
            {
                response = mutualfundData,
                totalAmount = totalAmount,
                totalBalanceUnit = totalPurchaseunit,
            };

            return mutualfundResponse;
        }
        #endregion

        #region Get Client Wise MF Transaction Summary
        public async Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummary(int userId)
        {
            var mfTransaction = await _context.TblMftransactions.Where(x => x.Userid == userId).ToListAsync();
            var mfSummary = mfTransaction.GroupBy(x => x.Schemename).ToList();

            return mfSummary;
        }
        #endregion

        #region Get Client Wise MF Transaction Summary By Category
        public async Task<List<IGrouping<string?, TblMftransaction>>> GetMFTransactionSummaryByCategory(int userId)
        {
            var mfTransaction = await _context.TblMftransactions.Where(x => x.Userid == userId).Include(x => x.TblMfSchemeMaster).ToListAsync();
            var mfSummary = mfTransaction.GroupBy(x => x.TblMfSchemeMaster.SchemeCategorytype).ToList();

            return mfSummary;
        }
        #endregion

        #region Get All Client  MF Transaction Summary
        public async Task<List<IGrouping<string?, TblMftransaction>>> GetAllCLientMFSummary(DateTime fromDate, DateTime toDate)
        {
            var mfTransaction = await _context.TblMftransactions.Where(x => x.Date >= fromDate && x.Date <= toDate).Include(x => x.TblMfSchemeMaster).ToListAsync();
            var mfSummary = mfTransaction.GroupBy(x => x.Username).ToList();

            return mfSummary;
        }
        #endregion

        #region Get All MF User Name
        public async Task<Response<TblMftransaction>> GetMFUserName(string? searchingParams, SortingParams sortingParams)
        {
            var mfUser = await _context.TblMftransactions.ToListAsync();
            var userName = mfUser.DistinctBy(x => x.Userid).AsQueryable();
            double pageCount = 0;

            if(searchingParams != null)
            {
                userName = _context.Search<TblMftransaction>(searchingParams);
            }

            pageCount = Math.Ceiling(userName.Count() / sortingParams.PageSize);

            //Apply sorting
            var sortingData = SortingExtensions.ApplySorting(userName, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(userName, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var responseData = new Response<TblMftransaction>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return responseData;
        }
        #endregion

        #region Display Scheme List
        public async Task<Response<TblMftransaction>> GetSchemeName(int userId, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId).ToList();
            var schemeName = mftransactions.DistinctBy(x => x.Schemename).AsQueryable();

            if (searchingParams != null)
                schemeName = _context.Search<TblMftransaction>(searchingParams);

            pageCount = Math.Ceiling(schemeName.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(schemeName, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var schemeNameResponse = new Response<TblMftransaction>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = sortingParams.PageNumber,
                    CurrentPage = (int)pageCount
                }
            };

            return schemeNameResponse;
        }
        #endregion

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
    }
}
