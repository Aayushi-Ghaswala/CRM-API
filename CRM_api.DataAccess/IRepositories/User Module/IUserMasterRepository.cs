using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserMasterRepository
    {
        Task<Response<TblUserMaster>> GetUsers(string filterString, string search, SortingParams sortingParams);
        Task<TblUserMaster> GetUserMasterbyId(int id, int? month = null, int? year = null, bool isCurrent = false);
        Task<Dictionary<string,int>> GetUserCount();
        Task<Response<TblUserCategoryMaster>> GetUserCategories(string search, SortingParams sortingParams);
        Task<List<TblUserMaster>> GetUsersByCategoryId(int categoryId, int? month, int? year, string search, bool isCurrent = false);
        Task<TblUserCategoryMaster> GetCategoryByName(string name);
        TblUserMaster GetUserByUserPan(string UserPan);
        int PanOrAadharExist(int? id, string? pan, string? aadhar);
        Task<List<TblUserMaster>> GetUsersForCSV(string filterString, string search, SortingParams sortingParams);
        Task<TblUserMaster> AddUser(TblUserMaster userMaster);
        Task<int> UpdateUser(TblUserMaster userMaster);
        Task<int> DeactivateUser(int id);
        int GetUserIdByUserPan(string UserPan);
        Task<TblUserMaster> GetUserByEmail(string email);
        Task<List<TblUserMaster>> GetUserByParentId(int? userId, DateTime date);
    }
}
