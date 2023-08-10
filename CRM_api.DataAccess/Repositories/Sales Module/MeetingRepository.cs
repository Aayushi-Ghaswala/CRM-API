using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly CRMDbContext _context;

        public MeetingRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Meetings
        public async Task<Response<TblMeetingMaster>> GetMeetings(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblMeetingMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblMeetingMaster>(search).Where(x => x.IsDeleted != true).Include(x => x.TblUserMaster).Include(x => x.Participants).Include(x => x.TblLeadMaster).Include(x => x.Attachments).AsQueryable();
            }
            else
            {
                filterData = _context.TblMeetingMasters.Where(x => x.IsDeleted != true).Include(x => x.TblUserMaster).Include(x => x.TblLeadMaster).Include(x => x.Participants).ThenInclude(x => x.TblUserMaster).Include(x => x.Participants).ThenInclude(x => x.TblLeadMaster).Include(x => x.Attachments).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var meetingResponse = new Response<TblMeetingMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return meetingResponse;
        }

        #endregion

        #region Get Meeting by Id
        public async Task<TblMeetingMaster> GetMeetingById(int id)
        {
            var meeting = await _context.TblMeetingMasters.AsNoTracking().Include(x => x.TblUserMaster).Include(x => x.TblLeadMaster).Include(x => x.Participants).ThenInclude(x => x.TblUserMaster).Include(x => x.Participants).ThenInclude(x => x.TblLeadMaster).Include(x => x.Attachments).FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return meeting;
        }

        #endregion

        #region Get Meeting by Purpose
        public async Task<TblMeetingMaster> GetMeetingByPurpose(string purpose)
        {
            var meeting = await _context.TblMeetingMasters.FirstAsync(x => x.Purpose.ToLower().Contains(purpose.ToLower()) && x.IsDeleted != true);
            return meeting;
        }

        #endregion

        #region Get Meeting by Lead
        public async Task<Response<TblMeetingMaster>> GetMeetingByLeadId(string search, SortingParams sortingParams, int leadId)
        {
            double pageCount = 0;

            var filterData = new List<TblMeetingMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblMeetingMaster>(search).Where(x => x.LeadId == leadId && x.IsDeleted != true).Include(x => x.TblUserMaster).Include(x => x.Participants).Include(x => x.TblLeadMaster).Include(x => x.Attachments).AsQueryable();
            }
            else
            {
                filterData = _context.TblMeetingMasters.Where(x => x.LeadId == leadId && x.IsDeleted != true).Include(x => x.TblUserMaster).Include(x => x.Participants).ThenInclude(x => x.TblUserMaster).Include(x => x.TblLeadMaster).Include(x => x.Attachments).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var meetingResponse = new Response<TblMeetingMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return meetingResponse;
        }

        #endregion

        #region Get User Wise Meetings
        public async Task<List<TblMeetingMaster>> GetUserWiseMeetings(int? userId, DateTime date)
        {
            var meetings = await _context.TblMeetingMasters.Where(x => (userId == null || x.MeetingBy == userId) && x.IsDeleted != true && x.DateOfMeeting.Month >= date.Month && x.DateOfMeeting.Year >= date.Year
                                                  && x.DateOfMeeting.Month <= DateTime.Now.Month && x.DateOfMeeting.Year <= DateTime.Now.Year).ToListAsync();

            return meetings;
        }
        #endregion

        #region Get User Wise Meetings By Date
        public async Task<List<TblMeetingMaster>> GetUserWiseMeetingsSchedule(int? userId)
        {
            var date = DateTime.Now.AddDays(6);
            var meetings = await _context.TblMeetingMasters.Where(x => (userId == null || x.MeetingBy == userId) && x.IsDeleted != true && x.DateOfMeeting.Date <= date.Date
                                                 && x.DateOfMeeting.Date >= DateTime.Now.Date).ToListAsync();

            return meetings;
        }
        #endregion

        #region Add Meeting
        public async Task<TblMeetingMaster> AddMeeting(TblMeetingMaster meetingRequest)
        {
            if (_context.TblMeetingMasters.Any(x => x.Purpose == meetingRequest.Purpose))
                return null;

            _context.TblMeetingMasters.Add(meetingRequest);
            await _context.SaveChangesAsync();
            return meetingRequest;
        }

        #endregion

        #region Add Meeting Attachments
        public async Task<int> AddMeetingAttachments(List<TblMeetingAttachment> attachments)
        {
            _context.TblMeetingAttachments.AddRange(attachments);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Meeting
        public async Task<int> UpdateMeeting(TblMeetingMaster meeting)
        {
            var meetings = _context.TblMeetingMasters.AsNoTracking().Where(x => x.Id == meeting.Id);

            if (meetings == null) return 0;

            _context.TblMeetingMasters.Update(meeting);
            return await _context.SaveChangesAsync();
        }

        #endregion

        #region Deactivate Meeting
        public async Task<int> DeactivateMeeting(int id)
        {
            var meeting = await _context.TblMeetingMasters.FindAsync(id);

            if (meeting == null) return 0;

            meeting.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }

        #endregion
    }
}