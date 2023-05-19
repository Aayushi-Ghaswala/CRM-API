using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserMasterRepository
    {
        Task<Response<TblUserMaster>> GetUsers(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<TblUserMaster> GetUserMasterbyId(int id);
        Task<Response<TblUserCategoryMaster>> GetUserCategories(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<Response<TblUserMaster>> GetUsersByCategoryId(int categoryId, Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<TblUserCategoryMaster> GetCategoryByName(string name);
        Task<int> AddUser(TblUserMaster userMaster);
        Task<int> UpdateUser(TblUserMaster userMaster);
        Task<int> DeactivateUser(int id);
    }
}
