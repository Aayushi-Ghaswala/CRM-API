using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.User_Module
{
    public class UserDashboardRepository : IUserDashboardRepository
    {
        private readonly CRMDbContext _context;

        public UserDashboardRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get New User And Client By Date Range
        public async Task<(List<TblUserMaster>, List<TblUserMaster>)> GetNewUserClient(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var newUsers = await _context.TblUserMasters.Where(x => ((fromDate == null && toDate == null) || (x.UserDoj.Value.Date >= fromDate.Value.Date && x.UserDoj.Value.Date <= toDate.Value.Date)) && x.UserIsactive == true).ToListAsync();

            var newClientList = new List<TblUserMaster>();

            var stClientNames = _context.TblStockData.Select(x => x.StClientname.ToLower()).Distinct().ToList();
            var newSTClient = newUsers.Where(x => stClientNames.Contains(x.UserName.ToLower())).ToList();
            newClientList.AddRange(newSTClient);

            var mfUserIds = _context.TblMftransactions.Select(x => x.Userid).Distinct().ToList();
            var newMFCLient = newUsers.Where(x => mfUserIds.Contains(x.UserId)).ToList();
            newClientList.AddRange(newMFCLient);

            var insUserIds = _context.TblInsuranceclients.Select(x => x.InsUserid).Distinct().ToList();
            var newInsClients = newUsers.Where(x => insUserIds.Contains(x.UserId)).ToList();
            newClientList.AddRange(newInsClients);

            var mgainUserIds = _context.TblMgaindetails.Select(x => x.MgainUserid).Distinct().ToList();
            var newMgainClients = newUsers.Where(x => mgainUserIds.Contains(x.UserId)).ToList();
            newClientList.AddRange(newMgainClients);

            var loanUserIds = _context.TblLoanMasters.Select(x => x.UserId).Distinct().ToList();
            var newLoanClients = newUsers.Where(x => loanUserIds.Contains(x.UserId)).ToList();
            newClientList.AddRange(newLoanClients);

            newClientList = newClientList.DistinctBy(x => x.UserId).ToList();

            return (newUsers, newClientList);
        }
        #endregion
    }
}
