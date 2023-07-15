
using CRM_api.DataAccess.Context;
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
            switch (assignTo)
            {
                case null:
                    filterData = _context.TblLeadMasters.Where(x => x.IsDeleted != true)
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
                    break;
                case 0:
                   filterData = _context.TblLeadMasters.Where(x => (x.AssignedTo == null || x.AssignedTo == 0) && x.IsDeleted != true)
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
                    break;
                default:
                    filterData = _context.TblLeadMasters.Where(x => x.AssignedTo == assignTo && x.IsDeleted != true)
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
                    break;
            }

            if (search != null)
            {
                switch (assignTo)
                {
                    case null:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                    case 0:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => (x.AssignedTo == null || x.AssignedTo == 0) && x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                    default:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => x.AssignedTo == assignTo && x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                }
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

            var filterData = _context.TblInvesmentTypes.AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblInvesmentType>(search).AsQueryable();
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
                                                    .Include(x => x.CountryMaster).FirstAsync(x => x.Id == id && x.IsDeleted != true);
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

        
        #region Get Leads
        public async Task<List<TblLeadMaster>> GetLeadsForCSV(int? assignTo, string search, SortingParams sortingParams)
        {
            var filterData = new List<TblLeadMaster>().AsQueryable();
            switch (assignTo)
            {
                case null:
                    filterData = _context.TblLeadMasters.Where(x => x.IsDeleted != true)
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
                    break;
                case 0:
                    filterData = _context.TblLeadMasters.Where(x => (x.AssignedTo == null || x.AssignedTo == 0) && x.IsDeleted != true)
                                                     .Include(x => x.AssignUser)
                                                     .Include(x => x.ReferredUser)
                                                     .Include(x => x.CampaignMaster)
                                                     .Include(x => x.StatusMaster)
                                                     .Include(x => x.CityMaster)
                                                     .Include(x => x.StateMaster)
                                                     .Include(x => x.CountryMaster).AsQueryable();
                    break;
                default:
                    filterData = _context.TblLeadMasters.Where(x => x.AssignedTo == assignTo && x.IsDeleted != true)
                                                    .Include(x => x.AssignUser)
                                                    .Include(x => x.ReferredUser)
                                                    .Include(x => x.CampaignMaster)
                                                    .Include(x => x.StatusMaster)
                                                    .Include(x => x.CityMaster)
                                                    .Include(x => x.StateMaster)
                                                    .Include(x => x.CountryMaster).AsQueryable();
                    break;
            }

            if (search != null)
            {
                switch (assignTo)
                {
                    case null:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                    case 0:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => (x.AssignedTo == null || x.AssignedTo == 0) && x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                    default:
                        filterData = _context.Search<TblLeadMaster>(search).Where(x => x.AssignedTo == assignTo && x.IsDeleted != true)
                                                        .Include(x => x.AssignUser)
                                                        .Include(x => x.ReferredUser)
                                                        .Include(x => x.CampaignMaster)
                                                        .Include(x => x.StatusMaster)
                                                        .Include(x => x.CityMaster)
                                                        .Include(x => x.StateMaster)
                                                        .Include(x => x.CountryMaster).AsQueryable();
                        break;
                }
            }

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending).ToList();

            return sortedData;
        }
        #endregion

        #region Check MobileNo Exist in Lead
        public int CheckMobileExist(int? id, string mobileNo)
        {
            if (id is not null)
            {
                if (_context.TblLeadMasters.Any(x => x.Id != id && x.MobileNo == mobileNo && !x.IsDeleted))
                    return 0;
            }
            else
            {
                if (_context.TblLeadMasters.Any(x => x.MobileNo == mobileNo && !x.IsDeleted))
                    return 0;
            }
            return 1;
        }
        #endregion

        #region Add Lead
        public async Task<int> AddLead(TblLeadMaster lead)
        {
            if (_context.TblLeadMasters.Any(x => x.MobileNo == lead.MobileNo && !x.IsDeleted))
                return 0;

            lead.CreatedAt = DateTime.Now.Date;
            _context.TblLeadMasters.Add(lead);
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

            if(lead == null) return 0;

            lead.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}