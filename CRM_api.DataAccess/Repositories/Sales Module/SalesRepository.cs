using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class SalesRepository : ISalesRepository
    {
        private readonly CRMDbContext _context;

        public SalesRepository(CRMDbContext context)
        {
            _context = context;
        }

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
    }
}
