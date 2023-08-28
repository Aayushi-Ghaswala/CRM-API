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
        Task<Response<TblUserMaster>> GetUsersByCategoryId(int categoryId, int? month, int? year, string search, SortingParams sortingParams, bool isCurrent = false);
        TblUserMaster GetUserByUserPan(string UserPan);
        int PanOrAadharExist(int? id, string? pan, string? aadhar);
        Task<List<TblUserMaster>> GetUsersForCSV(string filterString, string search, SortingParams sortingParams);
        Task<TblUserMaster> GetUserByName(string name);
        Task<Response<TblFamilyMember>> GetFamilyMemberByUserId(int userId, string? search, SortingParams sortingParams);
        Task<Response<TblFamilyMember>> GetRelativeAccessByUserId(int userId, string? search, SortingParams sortingParams);
        Task<TblUserMaster> AddUser(TblUserMaster userMaster);
        Task<int> UpdateUser(TblUserMaster userMaster);
        Task<int> UpdateRelativeAccess(int id, bool isDisable);
        Task<int> DeactivateUser(int id);
        int GetUserIdByUserPan(string UserPan);
        Task<TblUserMaster> GetUserByWorkEmail(string email);
        Task<List<TblUserMaster>> GetUserByParentId(int? userId, DateTime date);
        Task<List<TblUserMaster>> GetUserWhichClientCodeNotNull();
    }
}
