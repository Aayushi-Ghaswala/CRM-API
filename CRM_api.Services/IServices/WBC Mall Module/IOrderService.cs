using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.IServices.WBC_Mall_Module
{
    public interface IOrderService
    {
        Task<ResponseDto<OrderDto>> GetOrderAsync(int? statusId, string? search, SortingParams sortingParams);
        Task<(string, int)> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
    }
}
