using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.WBC_Mall_Module
{
    public interface IOrderRepository
    {
        Task<Response<TblOrder>> GetOrders(int? statusId, string? search, SortingParams sortingParams);
        Task<TblOrder> GetOrderById(int id);
        Task<int> CheckTrackingNoExist(int id, string trackingNo);
        Task<int> UpdateOrder(TblOrder order);
    }
}
