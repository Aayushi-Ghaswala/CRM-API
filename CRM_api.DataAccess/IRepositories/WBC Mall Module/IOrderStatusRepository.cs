using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.WBC_Mall_Module
{
    public interface IOrderStatusRepository
    {
        Task<Response<TblOrderStatus>> GetOrderStatuses(string? search, SortingParams sortingParams);
        Task<int> AddOrderStatus(TblOrderStatus tblOrderStatus);
        Task<int> UpdateOrderStatus(TblOrderStatus tblOrderStatus);
        Task<int> DeActivateOrderStatus(int id);
    }
}
