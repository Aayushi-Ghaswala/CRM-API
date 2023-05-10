using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel;

namespace CRM_api.DataAccess.IRepositories
{
    public interface IUserMasterRepository
    {
        Task<int> AddUser(TblUserMaster userMaster);
        Task<TblUserMaster> GetUserMasterbyId(int id);
        Task<int> UpdateUser(TblUserMaster userMaster);
        Task<UserResponse> GetUsers(int page, int catId);
        Task<IEnumerable<TblUserCategoryMaster>> GetUserCategories();
        Task<int> GetCategoryIdByName(string name);
    }
}
