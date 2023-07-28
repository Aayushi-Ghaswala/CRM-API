using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly CRMDbContext _context;

        public CampaignRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Campaigns
        public async Task<Response<TblCampaignMaster>> GetCampaigns(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblCampaignMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblCampaignMaster>(search).Where(x => x.IsDeleted != true)
                                                        .Include(x => x.TblSourceMaster)
                                                        .Include(x => x.TblSourceTypeMaster)
                                                        .Include(x => x.TblStatusMaster)
                                                        .Include(x => x.TblUserMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblCampaignMasters.Where(x => x.IsDeleted != true)
                                                        .Include(x => x.TblSourceMaster)
                                                        .Include(x => x.TblSourceTypeMaster)
                                                        .Include(x => x.TblStatusMaster)
                                                        .Include(x => x.TblUserMaster).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var campaignResponse = new Response<TblCampaignMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return campaignResponse;
        }
        #endregion

        #region Get Campaign by Id
        public async Task<TblCampaignMaster> GetCampaignById(int id)
        {
            var campaign = await _context.TblCampaignMasters.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return campaign;
        }
        #endregion

        #region Get Campaign by Name
        public async Task<TblCampaignMaster> GetCampaignByName(string Name)
        {
            var campaign = await _context.TblCampaignMasters.FirstAsync(x => x.Name.ToLower().Contains(Name.ToLower()) && x.IsDeleted != true);
            return campaign;
        }
        #endregion

        #region Add Campaign
        public async Task<int> AddCampaign(TblCampaignMaster campaign)
        {
            if (_context.TblCampaignMasters.Any(x => x.Name == campaign.Name))
                return 0;

            campaign.StartDate = campaign.StartDate.Date;
            campaign.EndDate = campaign.EndDate.Date;
            _context.TblCampaignMasters.Add(campaign);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Campaign
        public async Task<int> UpdateCampaign(TblCampaignMaster campaign)
        {
            var campaigns = _context.TblCampaignMasters.AsNoTracking().Where(x => x.Id == campaign.Id);

            if (campaigns == null) return 0;

            _context.TblCampaignMasters.Update(campaign);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Campaign
        public async Task<int> DeactivateCampaign(int id)
        {
            var campaign = await _context.TblCampaignMasters.FindAsync(id);

            if(campaign == null) return 0;

            campaign.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}