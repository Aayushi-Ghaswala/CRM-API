using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;
using CRM_api.Services.Helper.ConstantValue;
using CRM_api.Services.IServices.WBC_Mall_Module;

namespace CRM_api.Services.Services.WBC_Mall_Module
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMallProductRepository _mallProductRepository;
        private readonly IMapper _mapper;
        private const string delivery = "delivery";

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IMallProductRepository mallProductRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _mallProductRepository = mallProductRepository;
        }

        #region Get Orders
        public async Task<ResponseDto<OrderDto>> GetOrderAsync(int? statusId, string? search, SortingParams sortingParams)
        {
            var orders = await _orderRepository.GetOrders(statusId, search, sortingParams);
            var mappedOrders = _mapper.Map<ResponseDto<OrderDto>>(orders);

            return mappedOrders;
        }
        #endregion

        #region Update Order
        public async Task<(string, int)> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await _orderRepository.GetOrderById(updateOrderDto.OrderId);
            if (order is not null && order.TblOrderStatus is not null)
            {
                if (order.TblOrderStatus.Statusname.ToLower().Equals(updateOrderDto.TblOrderStatus.Statusname.ToLower()))
                    return ($"Order is already {updateOrderDto.TblOrderStatus.Statusname.ToLower()}.", 0);

                if (order.TblOrderStatus.Statusname.ToLower().Equals(OrderStatusConstant.cancelled))
                    return ("You can't update order after it's cancelled.", 0);

                var updateProductList = new List<TblWbcMallProduct>();
                var mappedUpdateOrderStatus = _mapper.Map<TblOrderStatus>(updateOrderDto.TblOrderStatus);
                switch (updateOrderDto.TblOrderStatus.Statusname.ToLower())
                {
                    case OrderStatusConstant.cancelled:
                        if (!order.TblOrderStatus.Statusname.ToLower().Equals(OrderStatusConstant.shipped)
                            && !order.TblOrderStatus.Statusname.ToLower().Equals(OrderStatusConstant.delivered))
                        {
                            order.CancelledDate = DateTime.Now;

                            foreach (var product in order.TblOrderDetails)
                            {
                                var updateProduct = await _mallProductRepository.GetMallProductById(product.ProductId);
                                updateProduct.ProdAvailableQty += product.Quantity;
                                updateProductList.Add(updateProduct);
                            }
                        }
                        else
                            return ("You can't cancelled after order is delivered or shipped.", 0);
                        break;
                    case OrderStatusConstant.delivered:
                        if (order.DeliverType.ToLower() != delivery)
                            return ("You can't deliver this order", 0);

                        order.DeleveredDate = DateTime.Now;
                        order.Status = true;
                        break;
                    case OrderStatusConstant.shipped:
                        if (order.DeliverType.ToLower() != delivery)
                            return ("You can't ship this order", 0);

                        var exist = await _orderRepository.CheckTrackingNoExist(updateOrderDto.OrderId, updateOrderDto.TrackingNumber);
                        if (exist == 0)
                            return ("Traking number already in use.", 0);

                        order.ShipDate = DateTime.Now;
                        order.TrackingNumber = updateOrderDto.TrackingNumber;
                        break;
                    case OrderStatusConstant.readyForPickup:
                        order.ReadyToPickupDate = DateTime.Now;
                        break;
                    case OrderStatusConstant.packed:
                        order.PackedDate = DateTime.Now;
                        break;
                    case OrderStatusConstant.placed:
                        order.OrderDate = DateTime.Now;
                        break;
                }

                order.TblOrderStatus = mappedUpdateOrderStatus;
                if (updateProductList.Count > 0)
                    await _mallProductRepository.UpdateMallProduct(updateProductList);

                var flag = await _orderRepository.UpdateOrder(order);
                if (flag > 0)
                    return ($"Order {updateOrderDto.TblOrderStatus.Statusname.ToLower()} successfully.", 1);
                else
                    return ("Unable to update order status.", flag);
            }

            return ("Unable to update order status.", 0);
        }
        #endregion
    }
}
