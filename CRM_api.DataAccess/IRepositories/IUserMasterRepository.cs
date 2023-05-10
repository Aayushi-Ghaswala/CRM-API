using CRM_api.DataAccess.Model;

namespace CRM_api.DataAccess.IRepositories
{
    public interface IUserMasterRepository
    {
        Task<int> AddUser(UserMaster userMaster);
        Task<UserMaster> GetUserMasterbyId(int id);
        Task<int> UpdateUser(UserMaster userMaster);
        Task<IEnumerable<UserMaster>> GetUsers();
        Task<IEnumerable<UserCategoryMaster>> GetUserCategories();
    }
}
