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
        public async Task<List<TblMftransaction>> GetMFInSpecificDateForExistUser(DateTime? startDate, DateTime? endDate, int userId = 0)
        {
            if (endDate is not null)
            {
                var getData = await _context.TblMftransactions.Where(x => x.Date >= startDate && x.Date <= endDate).ToListAsync();
                return getData;
            }
            else
            {
                var getData = await _context.TblMftransactions.Where(x => x.Userid == userId && x.Date.Value.Month == startDate.Value.Month && x.Date.Value.Year == startDate.Value.Year).ToListAsync();
                return getData;
            }
        }
        #endregion 

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
        public async Task<BussinessResponse<TblMftransaction>> GetTblMftransactions(int userId, string? schemeName, string? folioNo
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate)
        {
            List<TblMftransaction> TblMftransaction = new List<TblMftransaction>();
            IQueryable<TblMftransaction> mftransactions = TblMftransaction.AsQueryable();
            double pageCount = 0;

            mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId &&
                                                        (startDate == null || x.Date >= startDate) &&
                                                        (endDate == null || x.Date <= endDate) &&
                                                        (schemeName == null || x.Schemename == schemeName) &&
                                                        (folioNo == null || x.Foliono == folioNo)).AsQueryable();

            var redeemptionUnit = mftransactions.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
            var redeemUnit = redeemptionUnit.Sum(x => x.Noofunit);
            var redeemAmount = redeemptionUnit.Sum(x => x.Invamount);

            var TotalpurchaseUnit = mftransactions.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
            var purchaseUnit = TotalpurchaseUnit.Sum(x => x.Noofunit);
            var purchaseAmount = TotalpurchaseUnit.Sum(x => x.Invamount);

            decimal? totalPurchaseunit = purchaseUnit - redeemUnit;

            if (searchingParams != null)
            {

                mftransactions = _context.Search<TblMftransaction>(searchingParams).Where(x => x.Userid == userId &&
                                                        (startDate == null || x.Date >= startDate) &&
                                                        (endDate == null || x.Date <= endDate) &&
                                                        (schemeName == null || x.Schemename == schemeName) &&
                                                        (folioNo == null || x.Foliono == folioNo)).AsQueryable();
            }
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
                totalPurchaseAmount = purchaseAmount,
                totalRedeemAmount = redeemAmount,
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
        public async Task<Response<UserNameResponse>> GetMFUserName(string? searchingParams, SortingParams sortingParams)
        {
            IQueryable<UserNameResponse> userName = new List<UserNameResponse>().AsQueryable();
            double pageCount = 0;

            if (searchingParams != null)
                userName = _context.Search<TblMftransaction>(searchingParams).Where(x => x.Username.ToLower().Contains(searchingParams.ToLower())).Select(x => new UserNameResponse { UserId = x.Userid, UserName = x.Username }).Distinct().AsQueryable();
            else userName = _context.TblMftransactions.Select(x => new UserNameResponse { UserId = x.Userid, UserName = x.Username }).Distinct().AsQueryable();

            pageCount = Math.Ceiling(userName.Count() / sortingParams.PageSize);

            //Apply sorting
            var sortingData = SortingExtensions.ApplySorting(userName, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortingData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var responseData = new Response<UserNameResponse>()
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

        #region Get Mutual Funds Total Amount By UserId
        public async Task<decimal?> GetMFTransactionByUserId(int userId)
        {
            var mfTransactions = await _context.TblMftransactions.Where(u => u.Userid == userId).ToListAsync();

            var redemptionUnit = mfTransactions.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
            var redemAmount = redemptionUnit.Sum(x => x.Invamount);

            var TotalpurchaseUnit = mfTransactions.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
            var purchaseAmount = TotalpurchaseUnit.Sum(x => x.Invamount);

            decimal? totalAmount = purchaseAmount - redemAmount;

            return totalAmount;
        }
        #endregion

        #region Get Mutual Funds SIP By userId
        public async Task<List<TblMftransaction>> GetMFTransactionSIPLumpsumByUserId(int? month, int? year, int userId)
        {
            List<TblMftransaction> mfTransactions = new List<TblMftransaction>();

            if (month is not null && year is not null)
                mfTransactions = await _context.TblMftransactions.Where(u => u.Userid == userId && (u.Transactiontype == "PIP (SIP)" || u.Transactiontype == "SIP")
                                                                        && u.Date.Value.Month == month && u.Date.Value.Year == year).ToListAsync();
            else
                mfTransactions = await _context.TblMftransactions.Where(u => u.Userid == userId && (u.Transactiontype == "PIP (SIP)" || u.Transactiontype == "SIP")
                                                                        && u.Date.Value.Month == DateTime.Now.Month && u.Date.Value.Year == DateTime.Now.Year).ToListAsync();

            return mfTransactions;
        }
        #endregion

        #region Get All Schemes
        public async Task<List<TblMfSchemeMaster>> GetAllMFScheme()
        {
            return await _context.TblMfSchemeMasters.ToListAsync();
        }
        #endregion

        #region Get Mutual Funds SIP
        public async Task<List<TblMftransaction>> GetMonthlyMFTransactionSIPLumpsum()
        {
            var mfTransactions = await _context.TblMftransactions.Where(u => (u.Transactiontype == "PIP (SIP)" || u.Transactiontype == "PIP")
                                                                        && u.Date.Value.Month == DateTime.Now.Month && u.Date.Value.Year == DateTime.Now.Year).ToListAsync();

            return mfTransactions;
        }
        #endregion

        #region Display Scheme List
        public async Task<Response<TblMftransaction>> GetSchemeName(int userId, string? folioNo, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var mftransactions = new List<TblMftransaction>();

            if (searchingParams != null)
                mftransactions = _context.Search<TblMftransaction>(searchingParams).Where(x => x.Userid == userId && (folioNo == null || x.Foliono == folioNo)).ToList();
            else mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && (folioNo == null || x.Foliono == folioNo)).ToList();

            var schemeName = mftransactions.DistinctBy(x => x.Schemename).AsQueryable();

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
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return schemeNameResponse;
        }
        #endregion

        #region Display Folio Number List
        public async Task<Response<TblMftransaction>> GetFolioNo(int userId, string? schemeName, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var mftransactions = new List<TblMftransaction>();
            IQueryable<TblMftransaction> folioNo = mftransactions.AsQueryable();

            if (searchingParams != null)
                mftransactions = _context.Search<TblMftransaction>(searchingParams).Where(x => x.Userid == userId && (schemeName == null || x.Schemename == schemeName)).ToList();
            else
                mftransactions = _context.TblMftransactions.Where(x => x.Userid == userId && (schemeName == null || x.Schemename == schemeName)).ToList();

            folioNo = mftransactions.DistinctBy(x => x.Foliono).AsQueryable();
            pageCount = Math.Ceiling(folioNo.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(folioNo, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var folioNumberResponse = new Response<TblMftransaction>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return folioNumberResponse;
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

        #region Add And Update MF Scheme
        public async Task<int> UpdateMFScheme(List<TblMfSchemeMaster> schemeMasters)
        {
            _context.TblMfSchemeMasters.UpdateRange(schemeMasters);
            return await _context.SaveChangesAsync(true);
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
