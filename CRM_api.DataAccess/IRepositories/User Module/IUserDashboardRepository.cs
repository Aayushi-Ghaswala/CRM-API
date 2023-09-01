using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserDashboardRepository 
    {
        Task<(List<TblUserMaster>, List<TblUserMaster>)> GetNewUserClient(DateTime? fromDate = null, DateTime? toDate = null);
    }
}
