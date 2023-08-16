using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MGain_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.MGain_Module
{
    public class MGainRepository : IMGainRepository
    {
        private readonly CRMDbContext _context;

        public MGainRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Monthly Mgain Detail by UserId 
        public async Task<int> GetMonthlyMGainDetailByUserId(int userId, DateTime date)
        {
            var mgainCount = await _context.TblMgaindetails.Where(x => x.MgainUserid == userId && x.Date.Value.Month == date.Month && x.Date.Value.Year == date.Year).CountAsync();

            return mgainCount;
        }
        #endregion

        #region Get All MGain Details
        public async Task<MGainBussinessResponse<TblMgaindetail>> GetMGainDetails(int? currencyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblMgaindetail> tblMgaindetails = new List<TblMgaindetail>();
            IQueryable<TblMgaindetail> mGainDetails = tblMgaindetails.AsQueryable();

            if (searchingParams != null)
                mGainDetails = _context.Search<TblMgaindetail>(searchingParams).Where(x => (currencyId == null || x.TblMgainPaymentMethods.Any(x => x.CurrancyId == currencyId)) && (type == null || x.MgainType == type) && (isClosed == null || x.MgainIsclosed == isClosed) && (fromDate == null || x.Date >= fromDate) && (toDate == null || x.Date <= toDate)).Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster).Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            else
                mGainDetails = _context.TblMgaindetails.Where(x => (currencyId == null || x.TblMgainPaymentMethods.Any(x => x.CurrancyId == currencyId)) && (type == null || x.MgainType == type) && (isClosed == null || x.MgainIsclosed == isClosed) && (fromDate == null || x.Date >= fromDate) && (toDate == null || x.Date <= toDate)).Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster).Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();

            pageCount = Math.Ceiling(mGainDetails.Count() / sortingParams.PageSize);

            var totalAmount = mGainDetails.Sum(x => x.MgainInvamt);
            var redemAmount = mGainDetails.Sum(x => x.MgainRedemamt);
            var remainingAmount = totalAmount - redemAmount;

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(mGainDetails, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mGainData = new Response<TblMgaindetail>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var responseMGain = new MGainBussinessResponse<TblMgaindetail>()
            {
                response = mGainData,
                totalAmount = totalAmount,
                redemAmount = redemAmount,
                remainingAmount = remainingAmount
            };

            return responseMGain;
        }
        #endregion

        #region Get All MGain Details For Monthly Non-Cumulative Interest Computation & Release
        public async Task<IQueryable<TblMgaindetail>> GetAllMGainDetailsMonthly(int? schemeId, string? searchingParams, SortingParams sortingParams, string mgainType, DateTime date)
        {
            double pageCount = 0;
            List<TblMgaindetail> mGainDetails = new List<TblMgaindetail>();
            IQueryable<TblMgaindetail> filterData = mGainDetails.AsQueryable();

            if (searchingParams is not null)
                filterData = _context.Search<TblMgaindetail>(searchingParams).Where(x => x.MgainType.ToLower() == mgainType.ToLower() && x.Date < date && (schemeId == null || x.MgainSchemeid == schemeId)).Include(x => x.TblMgainSchemeMaster).Include(x => x.TblMgainPaymentMethods).AsQueryable();
            else
                filterData = _context.TblMgaindetails.Where(x => x.MgainType.ToLower() == mgainType.ToLower() && x.Date < date && (schemeId == null || x.MgainSchemeid == schemeId)).Include(x => x.TblMgainSchemeMaster).Include(x => x.TblMgainPaymentMethods).AsQueryable();

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            return sortedData;
        }
        #endregion

        #region Get MGain By Id
        public async Task<TblMgaindetail> GetMGainDetailById(int id)
        {
            var mGain = await _context.TblMgaindetails.Where(x => x.Id == id).AsNoTracking().Include(x => x.TblMgainSchemeMaster).Include(x => x.TblUserMaster)
                                    .Include(x => x.EmployeeMaster).Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster).FirstOrDefaultAsync();
            return mGain;
        }
        #endregion

        #region Get Payment Details By MGain Id
        public async Task<List<TblMgainPaymentMethod>> GetPaymentByMGainId(int mGainId)
        {
            var mGainPayment = await _context.TblMgainPaymentMethods.Where(x => x.Mgainid == mGainId).Include(x => x.TblMgainCurrancyMaster).ToListAsync();
            return mGainPayment;
        }
        #endregion

        #region Get Payment by Id
        public async Task<TblMgainPaymentMethod> GetPaymentById(int id)
        {
            var mGainPayment = await _context.TblMgainPaymentMethods.Where(x => x.Id == id).FirstOrDefaultAsync();
            return mGainPayment;
        }
        #endregion

        #region Get MGain Ledger For Interest Certificate By User Id
        public async Task<List<TblAccountTransaction>> GetMGainAccTransactionByUserId(int userId, DateTime? startDate, DateTime? endDate)
        {
            var transactionDetails = await _context.TblAccountTransactions.Where(t => t.DocUserid == userId && t.DocDate >= startDate && t.DocDate <= endDate && (t.Mgainid != null || t.Mgainid != 0)).OrderBy(x => x.DocDate)
                                                   .Include(x => x.TblMgaindetail).ToListAsync();

            return transactionDetails;
        }
        #endregion

        #region Get MGain Cumulative Details In Date Range
        public async Task<IQueryable<TblMgaindetail>> GetMGainCumulativeDetails(int fromYear, int toYear, int? schemeId, string? search, SortingParams sortingParams, string mgainType)
        {
            List<TblMgaindetail> details = new List<TblMgaindetail>();
            IQueryable<TblMgaindetail> filterData = details.AsQueryable();

            if (search is not null)
                filterData = _context.Search<TblMgaindetail>(search).Where(x => x.Date.Value.Year >= fromYear && x.Date.Value.Year <= toYear && x.MgainType.ToLower().Equals(mgainType.ToLower()) && (schemeId == null || x.MgainSchemeid == schemeId)).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            else
                filterData = _context.TblMgaindetails.Where(x => x.Date.Value.Year >= fromYear && x.Date.Value.Year <= toYear && x.MgainType.ToLower().Equals(mgainType.ToLower()) && (schemeId == null || x.MgainSchemeid == schemeId)).Include(x => x.TblMgainSchemeMaster).AsQueryable();

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            return sortedData;
        }
        #endregion

        #region Get All Project
        public async Task<Response<TblProjectMaster>> GetAllProject(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<TblProjectMaster> projects = new List<TblProjectMaster>().AsQueryable();
            
            if (searchingParams != null)
            {
                projects = _context.Search<TblProjectMaster>(searchingParams).Where(x => x.IsActive == true).Include(x => x.TblPlotMasters).AsQueryable(); ;
            }
            else
            {
                projects = _context.TblProjectMasters.Where(x => x.IsActive == true).Include(x => x.TblPlotMasters).AsQueryable();
            }

            pageCount = Math.Ceiling(projects.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedProject = SortingExtensions.ApplySorting(projects, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedProject, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var responseProjects = new Response<TblProjectMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return responseProjects;
        }
        #endregion

        #region Get Project By Project Name
        public async Task<TblProjectMaster> GetProjectByProjectName(string projectName)
        {
            var project = await _context.TblProjectMasters.Where(x => x.Name.ToLower() == projectName.ToLower()).FirstOrDefaultAsync();
            return project;
        }
        #endregion

        #region Get All Plot By ProjectId
        public async Task<List<TblPlotMaster>> GetPlotsByProjectId(int projectId, int? plotId)
        {
            double pageCount = 0;
            var plots = _context.TblPlotMasters.Where(x => x.ProjectId == projectId).Include(x => x.TblProjectMaster).ToList();

            if (plotId != 0)
            {
                var updatePlot = plots.Where(x => x.Id == plotId).FirstOrDefault();

                if (updatePlot != null)
                {
                    plots.Remove(updatePlot);
                    plots.Insert(0, updatePlot);
                }
            }

            return plots;
        }
        #endregion

        #region Get MGain Details By UserId
        public async Task<List<TblMgaindetail>> GetMGainDetailsByUserId(int UserId)
        {
            var mGainDetails = await _context.TblMgaindetails.Where(x => x.MgainUserid == UserId).Include(x => x.TblMgainPaymentMethods)
                .Include(x => x.TblMgainSchemeMaster).ToListAsync();
            return mGainDetails;
        }
        #endregion

        #region Get Mgain Account Transaction by Mgain Id
        public async Task<List<TblAccountTransaction>> GetAccountTransactionByMgainId(int? mGainId, int? month, int? year)
        {
            List<TblAccountTransaction> accountTransactions = new List<TblAccountTransaction>();

            if (mGainId != 0)
            {
                accountTransactions = await _context.TblAccountTransactions.Where(x => x.Mgainid == mGainId).ToListAsync();
            }
            else if (month != 0 && year != 0)
            {
                accountTransactions = await _context.TblAccountTransactions.Where(x => x.DocDate.Value.Month == month && x.DocDate.Value.Year == year).ToListAsync();
            }
            return accountTransactions;
        }
        #endregion

        #region Get Account by UserId
        public TblAccountMaster GetAccountByUserId(int? userId, string? accountName)
        {
            TblAccountMaster account = new TblAccountMaster();

            account = _context.TblAccountMasters.FirstOrDefault(x => (userId != 0 && x.UserId == userId) || (accountName != null & accountName == accountName));

            return account;
        }
        #endregion

        #region Get All Currency 
        public async Task<List<TblMgainCurrancyMaster>> GetAllCurrencies()
        {
            var currancy = await _context.TblMgainCurrancyMasters.ToListAsync();
            return currancy;
        }
        #endregion

        #region Get Plot By Id
        public async Task<TblPlotMaster> GetPlotById(int? id)
        {
            var plot = await _context.TblPlotMasters.Where(x => x.Id == id).FirstOrDefaultAsync();
            return plot;
        }
        #endregion

        #region Get Plot By Project Name and Plot No.
        public async Task<TblPlotMaster> GetPlotByProjectAndPlotNo(string? projectName, string plotNo)
        {
            var plot = await _context.TblPlotMasters.Where(x => x.TblProjectMaster.Name == projectName && x.PlotNo == plotNo).FirstOrDefaultAsync();
            if (plot == null)
                return new TblPlotMaster();
            return plot;
        }
        #endregion

        #region Add MGain Details
        public async Task<TblMgaindetail> AddMGainDetails(TblMgaindetail mgainDetail)
        {
            _context.TblMgaindetails.Add(mgainDetail);
            await _context.SaveChangesAsync();
            return mgainDetail;
        }
        #endregion

        #region Add Payment Details
        public async Task<int> AddPaymentDetails(List<TblMgainPaymentMethod> tblMgainPayment)
        {
            _context.TblMgainPaymentMethods.AddRange(tblMgainPayment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add User Account
        public async Task<int> AddUserAccount(TblAccountMaster tblAccountMaster)
        {
            if (_context.TblAccountMasters.Any(x => x.UserId == tblAccountMaster.UserId && x.AccountName == tblAccountMaster.AccountName))
                return 0;
            else
            {
                await _context.TblAccountMasters.AddAsync(tblAccountMaster);
                return await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region Add MGain Interest Entry
        public int AddMGainInterest(List<TblAccountTransaction> tblAccountTransactions, DateTime? date)
        {
            var accountTransaction = tblAccountTransactions.First();
            if (_context.TblAccountTransactions.Any(x => x.DocType.ToLower().Equals(accountTransaction.DocType) && x.DocDate.Value.Month == date.Value.Month
                        && x.DocDate.Value.Year == date.Value.Year && x.Mgainid != null))
            {
                return 0;
            }

            _context.TblAccountTransactions.AddRange(tblAccountTransactions);
            return _context.SaveChanges();
        }
        #endregion

        #region Update MGain Details
        public async Task<int> UpdateMGainDetails(TblMgaindetail tblMgaindetail)
        {
            _context.TblMgaindetails.Update(tblMgaindetail);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update MGain Payment Details
        public async Task<int> UpdateMGainPayment(TblMgainPaymentMethod tblMgainPayment)
        {
            _context.TblMgainPaymentMethods.Update(tblMgainPayment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Plot Details
        public async Task<int> UpdatePlotDetails(List<TblPlotMaster> tblPlotMaster)
        {
            _context.TblPlotMasters.UpdateRange(tblPlotMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete MGain Payment Details
        public async Task<int> DeleteMGainPayment(TblMgainPaymentMethod tblMgainPayment)
        {
            _context.TblMgainPaymentMethods.Remove(tblMgainPayment);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
