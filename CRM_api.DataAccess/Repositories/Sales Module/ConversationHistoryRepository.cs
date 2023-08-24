using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class ConversationHistoryRepository : IConversationHistoryRepository
    {
        private readonly CRMDbContext _context;

        public ConversationHistoryRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Conversation Histories
        public async Task<Response<TblConversationHistoryMaster>> GetConversationHistory(int? meetingId, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblConversationHistoryMaster>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblConversationHistoryMaster>(search).Where(x => (meetingId == null || x.MeetingId == meetingId) && x.IsDeleted == false).Include(x => x.TblMeetingMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblConversationHistoryMasters.Where(x => (meetingId == null || x.MeetingId == meetingId) && x.IsDeleted == false).Include(x => x.TblMeetingMaster).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(filterData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var historyResponse = new Response<TblConversationHistoryMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return historyResponse;
        }
        #endregion

        #region Get Lead Wise Conversation History
        public async Task<Response<TblConversationHistoryMaster>> GetLeadWiseConversionHistory(int leadId, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblConversationHistoryMaster>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblConversationHistoryMaster>(search).Where(x => x.TblMeetingMaster.LeadId == leadId && x.IsDeleted == false).Include(x => x.TblMeetingMaster)
                                                                       .ThenInclude(x => x.TblUserMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblConversationHistoryMasters.Where(x => x.TblMeetingMaster.LeadId == leadId && x.IsDeleted == false).Include(x => x.TblMeetingMaster)
                                                                   .ThenInclude(x => x.TblUserMaster).AsQueryable();
            }

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var conversationResponse = new Response<TblConversationHistoryMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return conversationResponse;
        }
        #endregion

        #region Add Conversation History
        public async Task<int> AddConversationHistory(TblConversationHistoryMaster tblConversationHistoryMaster)
        {
            _context.TblConversationHistoryMasters.Add(tblConversationHistoryMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Conversation History
        public async Task<int> UpdateConversationHistory(TblConversationHistoryMaster tblConversationHistoryMaster)
        {
            var conversationHisotry = await _context.TblConversationHistoryMasters.Where(x => x.Id == tblConversationHistoryMaster.Id).AsNoTracking().FirstOrDefaultAsync();
            if (conversationHisotry is null)
                return 0;

            _context.TblConversationHistoryMasters.Update(tblConversationHistoryMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region DeActivate Conversation History
        public async Task<int> DeActivateConversationHistory(int id)
        {
            var conversationHistory = await _context.TblConversationHistoryMasters.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (conversationHistory is null)
                return 0;

            conversationHistory.IsDeleted = true;
            _context.TblConversationHistoryMasters.Update(conversationHistory);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
