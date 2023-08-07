using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserCategoryRepository
    {
        Task<Response<TblUserCategoryMaster>> GetUserCategories(string search, SortingParams sortingParams);
        Task<int> AddUserCategory(TblUserCategoryMaster tblUserCategory);
        Task<int> UpdateUserCategory(TblUserCategoryMaster tblUserCategory);
        Task<int> DeActivateUserCategory(int id);
    }
}
