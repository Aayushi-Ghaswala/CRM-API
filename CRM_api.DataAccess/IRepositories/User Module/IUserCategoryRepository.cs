using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.IRepositories.User_Module
{
    public interface IUserCategoryRepository
    {
        Task<int> AddUserCategory(TblUserCategoryMaster tblUserCategory);
        Task<int> UpdateUserCategory(TblUserCategoryMaster tblUserCategory);
        Task<int> DeActivateUserCategory(int id);
    }
}
