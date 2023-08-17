using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.WBC_Mall_Module
{
    public interface IMallProductRepository
    {
        Task<Response<TblWbcMallProduct>> GetMallProducts(int? catId, string? filterString, string? search, SortingParams sortingParams);
        Task<TblWbcMallProduct> GetMallProductById(int id);
        Task<TblWbcMallProduct> AddMallProduct(TblWbcMallProduct tblWbcMallProduct);
        Task<int> UpdateMallProduct(List<TblWbcMallProduct> tblWbcMallProducts);
        Task<int> DeleteProductImage(int id);
    }
}
