using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class MeetingParticipantRepository : IMeetingParticipantRepository
    {
        private readonly CRMDbContext _context;

        public MeetingParticipantRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get MeetingParticipants
        public async Task<Response<TblMeetingParticipant>> GetMeetingParticipants(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblMeetingParticipant>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblMeetingParticipant>(search).Where(x => x.IsDeleted != true)
                                                            .Include(x => x.TblMeetingMaster)
                                                            .Include(x => x.TblUserMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblMeetingParticipants.Where(x => x.IsDeleted != true)
                                                            .Include(x => x.TblMeetingMaster)
                                                            .Include(x => x.TblUserMaster).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var meetingParticipantResponse = new Response<TblMeetingParticipant>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return meetingParticipantResponse;
        }

        #endregion

        #region Get MeetingParticipant by Id
        public async Task<TblMeetingParticipant> GetMeetingParticipantById(int id)
        {
            var meetingParticipant = await _context.TblMeetingParticipants.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return meetingParticipant;
        }

        #endregion

        #region Add MeetingParticipant
        public async Task<int> AddMeetingParticipant(TblMeetingParticipant meetingParticipant)
        {
            if (_context.TblMeetingParticipants.Any(x => x.Id == meetingParticipant.Id))
                return 0;

            _context.TblMeetingParticipants.Add(meetingParticipant);
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Update MeetingParticipant
        public async Task<int> UpdateMeetingParticipant(TblMeetingParticipant meetingParticipant)
        {
            var meetingParticipants = _context.TblMeetingParticipants.AsNoTracking().Where(x => x.Id == meetingParticipant.Id);

            if (meetingParticipants == null) return 0;

            _context.TblMeetingParticipants.Update(meetingParticipant);
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Deactivate MeetingParticipant
        public async Task<int> DeactivateMeetingParticipant(int id)
        {
            var meetingParticipant = await _context.TblMeetingParticipants.FindAsync(id);

            if (meetingParticipant == null) return 0;

            meetingParticipant.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}