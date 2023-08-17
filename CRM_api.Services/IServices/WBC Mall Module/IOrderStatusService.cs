using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.IServices.WBC_Mall_Module
{
    public interface IOrderStatusService
    {
        Task<ResponseDto<OrderStatusDto>> GetOrderStatusesAsync(string? search, SortingParams sortingParams);
        Task<int> AddOrderStatusAsync(AddOrderStatusDto addOrderStatusDto);
        Task<int> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto);
        Task<int> DeActivateOrderStatusAsync(int id);
    }
}
