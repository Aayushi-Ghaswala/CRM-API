﻿using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class LeadRepository : ILeadRepository
    {
        private readonly CRMDbContext _context;

        public LeadRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Leads
        public async Task<Response<TblLeadMaster>> GetLeads(int? assignTo, string search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblLeadMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblLeadMaster>(search).Where(x => x.IsDeleted != true && (assignTo == null || (assignTo == 0 && (x.AssignedTo == null || x.AssignedTo == 0))
                                            || (assignTo != null && assignTo != 0 && x.AssignedTo == assignTo)))
                                            .Include(x => x.AssignUser)
                                            .Include(x => x.ReferredUser)
                                            .Include(x => x.CampaignMaster)
                                            .Include(x => x.StatusMaster)
                                            .Include(x => x.CityMaster)
                                            .Include(x => x.StateMaster)
                                            .Include(x => x.CountryMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblLeadMasters.Where(x => x.IsDeleted != true && (assignTo == null || (assignTo == 0 && (x.AssignedTo == null || x.AssignedTo == 0))
                                                    || (assignTo != null && assignTo != 0 && x.AssignedTo == assignTo)))
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
            }

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var leadResponse = new Response<TblLeadMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return leadResponse;
        }
        #endregion

        #region Get Investment Types
        public async Task<Response<TblInvesmentType>> GetInvestmentTypes(string search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblInvesmentType>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblInvesmentType>(search).AsQueryable();
            }
            else
            {
                filterData = _context.TblInvesmentTypes.AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var leadResponse = new Response<TblInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return leadResponse;
        }
        #endregion

        #region Get Lead by Id
        public async Task<TblLeadMaster> GetLeadById(int id)
        {
            var lead = await _context.TblLeadMasters.Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsNoTracking().FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return lead;
        }
        #endregion

        #region Get Investment by Id
        public TblInvesmentType GetInvestmentById(int id)
        {
            var lead = _context.TblInvesmentTypes.First(x => x.Id == id);
            return lead;
        }
        #endregion

        #region Get Lead by Name
        public async Task<TblLeadMaster> GetLeadByName(string Name)
        {
            var lead = await _context.TblLeadMasters.Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).FirstAsync(x => x.Name.ToLower().Contains(Name.ToLower()) && x.IsDeleted != true);
            return lead;
        }
        #endregion

        #region Get Leads For CSV
        public async Task<List<TblLeadMaster>> GetLeadsForCSV(int? assignTo, string search, SortingParams sortingParams)
        {
            var filterData = new List<TblLeadMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblLeadMaster>(search).Where(x => x.IsDeleted != true && (assignTo == null || (assignTo == 0 && (x.AssignedTo == null || x.AssignedTo == 0))
                                            || (assignTo != null && assignTo != 0 && x.AssignedTo == assignTo)))
                                            .Include(x => x.AssignUser)
                                            .Include(x => x.ReferredUser)
                                            .Include(x => x.CampaignMaster)
                                            .Include(x => x.StatusMaster)
                                            .Include(x => x.CityMaster)
                                            .Include(x => x.StateMaster)
                                            .Include(x => x.CountryMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblLeadMasters.Where(x => x.IsDeleted != true && (assignTo == null || (assignTo == 0 && (x.AssignedTo == null || x.AssignedTo == 0))
                                || (assignTo != null && assignTo != 0 && x.AssignedTo == assignTo)))
                                .Include(x => x.AssignUser)
                                .Include(x => x.ReferredUser)
                                .Include(x => x.CampaignMaster)
                                .Include(x => x.StatusMaster)
                                .Include(x => x.CityMaster)
                                .Include(x => x.StateMaster)
                                .Include(x => x.CountryMaster).AsQueryable();
            }

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending).ToList();

            return sortedData;
        }
        #endregion

        #region Check MobileNo Exist in Lead
        public int CheckMobileExist(int? id, string mobileNo)
        {
            if (_context.TblLeadMasters.Any(x => (id == null || x.Id != id) && x.MobileNo == mobileNo && !x.IsDeleted))
                return 0;
            return 1;
        }
        #endregion

        #region Get User wise Leads
        public async Task<List<TblLeadMaster>> GetUserwiseLeads(int? userId, int? campaignId, DateTime date)
        {
            var leads = await _context.TblLeadMasters.Where(x => (userId == null || x.ReferredBy == userId) && (campaignId == null || x.CampaignId == campaignId) && x.IsDeleted != true && x.CreatedAt.Month >= date.Month && x.CreatedAt.Year >= date.Year
                                                 && x.CreatedAt.Month <= DateTime.Now.Month && x.CreatedAt.Year <= DateTime.Now.Year).ToListAsync();

            return leads;
        }
        #endregion

        #region Get Leads By Date Range
        public async Task<(List<TblLeadMaster>, List<TblLeadMaster>)> GetLeadsByDateRange(int? assignTo, DateTime fromDate, DateTime toDate)
        {
            var leads = await _context.TblLeadMasters.Where(x => (assignTo == null || x.AssignedTo == assignTo) && x.CreatedAt.Date >= fromDate.Date && x.CreatedAt.Date <= toDate.Date && x.IsDeleted == false && x.UserId != null).Include(x => x.TblUserMaster).ToListAsync();

           var newClientLeadList = new List<TblLeadMaster>();

            var stClientNames = _context.TblStockData.Select(x => x.StClientname.ToLower()).Distinct().ToList();
            var newSTLeadClient = leads.Where(x => stClientNames.Contains(x.TblUserMaster.UserName.ToLower())).ToList();
            newClientLeadList.AddRange(newSTLeadClient);

            var mfUserIds = _context.TblMftransactions.Select(x => x.Userid).Distinct().ToList();
            var newMFLeadCLient = leads.Where(x => mfUserIds.Contains(x.TblUserMaster.UserId)).ToList();
            newClientLeadList.AddRange(newMFLeadCLient);

            var insUserIds = _context.TblInsuranceclients.Select(x => x.InsUserid).Distinct().ToList();
            var newInsLeadClients = leads.Where(x => insUserIds.Contains(x.TblUserMaster.UserId)).ToList();
            newClientLeadList.AddRange(newInsLeadClients);

            var mgainUserIds = _context.TblMgaindetails.Select(x => x.MgainUserid).Distinct().ToList();
            var newMgainLeadClients = leads.Where(x => mgainUserIds.Contains(x.TblUserMaster.UserId)).ToList();
            newClientLeadList.AddRange(newMgainLeadClients);

            var loanUserIds = _context.TblLoanMasters.Select(x => x.UserId).Distinct().ToList();
            var newLoanLeadClients = leads.Where(x => loanUserIds.Contains(x.TblUserMaster.UserId)).ToList();
            newClientLeadList.AddRange(newLoanLeadClients);

            newClientLeadList = newClientLeadList.DistinctBy(x => x.UserId).ToList();

            return (leads, newClientLeadList);
        }
        #endregion

        #region Add Lead
        public async Task<int> AddLead(List<TblLeadMaster> leads)
        {
            if (_context.TblLeadMasters.Any(x => leads.Select(x => x.MobileNo).Contains(x.MobileNo) && !x.IsDeleted))
                return 0;

            _context.TblLeadMasters.AddRange(leads);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Lead
        public async Task<int> UpdateLead(TblLeadMaster lead)
        {
            var leads = _context.TblLeadMasters.AsNoTracking().Where(x => x.Id == lead.Id);

            if (leads == null) return 0;

            lead.CreatedAt = DateTime.Now.Date;
            _context.TblLeadMasters.Update(lead);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Lead
        public async Task<int> DeactivateLead(int id)
        {
            var lead = await _context.TblLeadMasters.FindAsync(id);

            if (lead == null) return 0;

            lead.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}