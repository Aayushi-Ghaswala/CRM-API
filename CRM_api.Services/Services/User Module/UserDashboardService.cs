using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.User_Module;

namespace CRM_api.Services.Services.User_Module
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IUserDashboardRepository _userDashboardRepository;

        public UserDashboardService(IUserDashboardRepository userDashboardRepository)
        {
            _userDashboardRepository = userDashboardRepository;
        }

        #region Get New User And Client Count
        public async Task<List<NewUserClientCountDto>> GetNewUserClientCountAsync()
        {
            var newUserClientCountList = new List<NewUserClientCountDto>();
            var users = await _userDashboardRepository.GetNewUserClient();

            //Today 
            var currDate = DateTime.Now;
            var todayNewUsers = users.Item1.Where(x => x.UserDoj?.Date == currDate.Date).Count();
            var todayNewClients = users.Item2.Where(x => x.UserDoj?.Date == currDate.Date).Count();
            var todayNewUserClientCount = new NewUserClientCountDto("Today", todayNewUsers, todayNewClients);
            newUserClientCountList.Add(todayNewUserClientCount);

            //This Week
            DateTime startOfWeek = currDate.AddDays(-(int)currDate.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            var thisWeekNewUsers = users.Item1.Where(x => x.UserDoj?.Date >= startOfWeek.Date && x.UserDoj?.Date <= endOfWeek.Date).Count();
            var thisWeekNewClients = users.Item2.Where(x => x.UserDoj?.Date >= startOfWeek.Date && x.UserDoj?.Date <= endOfWeek.Date).Count();
            var thisWeekNewUserClientCount = new NewUserClientCountDto("This Week", thisWeekNewUsers, thisWeekNewClients);
            newUserClientCountList.Add(thisWeekNewUserClientCount);

            //This Month
            var thisMonthNewUsers = users.Item1.Where(x => x.UserDoj?.Month == currDate.Month && x.UserDoj?.Year == currDate.Year).Count();
            var thisMonthNewClients = users.Item2.Where(x => x.UserDoj?.Month == currDate.Month && x.UserDoj?.Year == currDate.Year).Count();
            var thisMonthNewUserClientCount = new NewUserClientCountDto("This Month", thisMonthNewUsers, thisMonthNewClients);
            newUserClientCountList.Add(thisMonthNewUserClientCount);

            //This Quarter
            var quarter = (currDate.Month - 1) / 3 + 1;
            DateTime quarterStartDate = new DateTime(currDate.Year, (quarter - 1) * 3 + 1, 1);
            DateTime quarterEndDate = quarterStartDate.AddMonths(3).AddDays(-1);
            var thisQuarterNewUsers = users.Item1.Where(x => x.UserDoj?.Date >= quarterStartDate.Date && x.UserDoj?.Date <= quarterEndDate.Date).Count();
            var thisQuarterNewClients = users.Item2.Where(x => x.UserDoj?.Date >= quarterStartDate.Date && x.UserDoj?.Date <= quarterEndDate.Date).Count();
            var thisQuarterNewUserClientCount = new NewUserClientCountDto("This Quarter", thisQuarterNewUsers, thisQuarterNewClients);
            newUserClientCountList.Add(thisQuarterNewUserClientCount);

            //This Year
            var thisYearNewUsers = users.Item1.Where(x => x.UserDoj?.Year == currDate.Year).Count();
            var thisYearNewClients = users.Item2.Where(x => x.UserDoj?.Year == currDate.Year).Count();
            var thisYearNewUserClientCount = new NewUserClientCountDto("This Year", thisYearNewUsers, thisMonthNewClients);
            newUserClientCountList.Add(thisYearNewUserClientCount);

            //All Time
            var allTimeNewUsers = users.Item1.Count();
            var allTimeNewClients = users.Item2.Count();
            var allTimeNewUserClientCount = new NewUserClientCountDto("All Time", allTimeNewUsers, allTimeNewClients);
            newUserClientCountList.Add(allTimeNewUserClientCount);

            return newUserClientCountList;
        }
        #endregion

        #region Get New User And Client Count Chart By DateRange
        public async Task<List<NewUserClientCountDto>> GetNewUserClientCountChartByDateRangeAsync(DateTime? fromDate, DateTime? toDate)
        {
            var users = await _userDashboardRepository.GetNewUserClient(fromDate, toDate);
            var newUserClientCountList = new List<NewUserClientCountDto>();
            var monthdiffrence = (12 * (toDate.Value.Year - fromDate.Value.Year) + toDate.Value.Month - fromDate.Value.Month) + 1;
           
            for (var i = 0; i < monthdiffrence; i++)
            {
                var date = fromDate.Value.AddMonths(i);
                var newUserCount = users.Item1.Where(x => x.UserDoj?.Month == date.Month && x.UserDoj?.Year == date.Year).Count();
                var newClientCount = users.Item2.Where(x => x.UserDoj?.Month == date.Month && x.UserDoj?.Year == date.Year).Count();
                var newUserClientCount = new NewUserClientCountDto(date.ToString("MMM-yyyy"), newUserCount, newClientCount);
                newUserClientCountList.Add(newUserClientCount);
            }

            return newUserClientCountList;
        }
        #endregion
    }
}
