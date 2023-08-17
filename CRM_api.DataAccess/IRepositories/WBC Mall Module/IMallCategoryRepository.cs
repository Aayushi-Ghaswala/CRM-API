using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.WBC_Mall_Module
{
    public interface IMallCategoryRepository
    {
        Task<Response<TblWbcMallCategory>> GetMallCategories(string? search, SortingParams sortingParams);
        Task<TblWbcMallCategory> GetMallCategoryById(int id);
        Task<TblWbcMallCategory> AddMallCategory(TblWbcMallCategory wbcMallCategory);
        Task<int> UpdateMallCategory(TblWbcMallCategory wbcMallCategory);
        Task<int> DeActivateMallCategory(int id);
    }
}
