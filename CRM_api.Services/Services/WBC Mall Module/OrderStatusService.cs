using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;

namespace CRM_api.Services.Services.WBC_Mall_Module
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IMapper _mapper;

        public OrderStatusService(IOrderStatusRepository orderStatusRepository, IMapper mapper)
        {
            _orderStatusRepository = orderStatusRepository;
            _mapper = mapper;
        }

        #region Get Order Statuses
        public async Task<ResponseDto<OrderStatusDto>> GetOrderStatusesAsync(string? search, SortingParams sortingParams)
        {
            var orderStatuses = await _orderStatusRepository.GetOrderStatuses(search, sortingParams);
            var mappedOrderStatuses = _mapper.Map<ResponseDto<OrderStatusDto>>(orderStatuses);

            return mappedOrderStatuses;
        }
        #endregion

        #region Add Order Status
        public async Task<int> AddOrderStatusAsync(AddOrderStatusDto addOrderStatusDto)
        {
            var orderStatus = _mapper.Map<TblOrderStatus>(addOrderStatusDto);
            return await _orderStatusRepository.AddOrderStatus(orderStatus);
        }
        #endregion

        #region Update Order Status
        public async Task<int> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var orderStatus = _mapper.Map<TblOrderStatus>(updateOrderStatusDto);
            return await _orderStatusRepository.UpdateOrderStatus(orderStatus);
        }
        #endregion

        #region De-Activate Order Status
        public async Task<int> DeActivateOrderStatusAsync(int id)
        {
            return await _orderStatusRepository.DeActivateOrderStatus(id);  
        }
        #endregion
    }
}
