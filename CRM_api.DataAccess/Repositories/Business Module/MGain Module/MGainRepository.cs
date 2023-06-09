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

        #region Get All MGain Details
        public async Task<MGainBussinessResponse<TblMgaindetail>> GetMGainDetails(int? currancyId, string? type, bool? isClosed, DateTime? fromDate, DateTime? toDate, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            List<TblMgaindetail> tblMgaindetails = new List<TblMgaindetail>();
            IQueryable<TblMgaindetail> mGainDetails = tblMgaindetails.AsQueryable();

            if (currancyId != null && type != null)
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Where(x => x.TblMgainPaymentMethods.Any(x => x.CurrancyId == currancyId) && x.MgainType == type)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.TblMgainPaymentMethods.Any(x => x.CurrancyId == currancyId)
                                             && x.MgainType == type && x.Date >= fromDate && x.Date <= toDate)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }
            else if (currancyId != null)
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Where(x => x.TblMgainPaymentMethods.Any(x => x.CurrancyId == currancyId))
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.TblMgainPaymentMethods.All(x => x.CurrancyId == currancyId)
                                             && x.Date >= fromDate && x.Date <= toDate).Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }
            else if (type != null)
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainType == type)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainType == type && x.Date >= fromDate && x.Date <= toDate)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }
            else if (isClosed is true)
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainIsclosed == true)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainIsclosed == true && x.Date >= fromDate && x.Date <= toDate)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }
            else if (isClosed is false)
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainIsclosed == false)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.MgainIsclosed == false && x.Date >= fromDate && x.Date <= toDate)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }
            else
            {
                if (fromDate == null && toDate == null)
                    mGainDetails = _context.TblMgaindetails.Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
                else
                    mGainDetails = _context.TblMgaindetails.Where(x => x.Date >= fromDate && x.Date <= toDate)
                                            .Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster)
                                            .Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster).Include(x => x.TblMgainSchemeMaster).AsQueryable();
            }

            if (searchingParams != null)
                mGainDetails = _context.Search<TblMgaindetail>(searchingParams).Include(x => x.TblUserMaster).Include(x => x.EmployeeMaster)
                                            .Include(x => x.TblMgainSchemeMaster).Include(x => x.TblMgainPaymentMethods).ThenInclude(x => x.TblMgainCurrancyMaster);

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

        #region Get All Project
        public async Task<Response<TblProjectMaster>> GetAllProject(string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var projects = _context.TblProjectMasters.Where(x => x.IsActive == true).Include(x => x.TblPlotMasters).AsQueryable();

            if (searchingParams != null)
            {
                projects = _context.Search<TblProjectMaster>(searchingParams);
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
        public async Task<Response<TblPlotMaster>> GetPlotsByProjectId(int projectId, decimal invAmount, string? searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;
            var plots = _context.TblPlotMasters.Where(x => x.ProjectId == projectId && x.Available_PlotValue >= invAmount).Include(x => x.TblProjectMaster).AsQueryable();

            if (searchingParams != null)
            {
                plots = _context.Search<TblPlotMaster>(searchingParams);
            }
            pageCount = Math.Ceiling(plots.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedPlots = SortingExtensions.ApplySorting(plots, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedPlots, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var responsePlots = new Response<TblPlotMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return responsePlots;
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
        public async Task<TblPlotMaster> GetPlotById(int id)
        {
            var plot = await _context.TblPlotMasters.Where(x => x.Id == id).FirstOrDefaultAsync();
            return plot;
        }
        #endregion

        #region Get Plot By Project Name and Plot No.
        public async Task<TblPlotMaster> GetPlotByProjectAndPlotNo(decimal? totalSqFt, string plotNo)
        {
            var plot = await _context.TblPlotMasters.Where(x => x.SqFt == totalSqFt && x.PlotNo == plotNo).FirstOrDefaultAsync();
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

        #region All Payment Details
        public async Task<int> AddPaymentDetails(List<TblMgainPaymentMethod> tblMgainPayment)
        {
            _context.TblMgainPaymentMethods.AddRange(tblMgainPayment);
            return await _context.SaveChangesAsync();
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
        public async Task<int> UpdatePlotDetails(TblPlotMaster tblPlotMaster)
        {
            _context.TblPlotMasters.Update(tblPlotMaster);
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
