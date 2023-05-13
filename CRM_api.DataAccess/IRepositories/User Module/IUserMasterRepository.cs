using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserMasterRepository
    {
        Task<int> AddUser(TblUserMaster userMaster);
        Task<TblUserMaster> GetUserMasterbyId(int id);
        Task<int> UpdateUser(TblUserMaster userMaster);
        Task<Response<TblUserMaster>> GetUsers(int page);
        Task<Response<TblUserCategoryMaster>> GetUserCategories(int page);
        Task<int> GetCategoryIdByName(string name);
        Task<Response<TblUserMaster>> GetUsersByCategoryId(int page, int catId);
    }
}
