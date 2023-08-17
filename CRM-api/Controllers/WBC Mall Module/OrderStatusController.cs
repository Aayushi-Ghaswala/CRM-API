using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.WBC_Mall_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        #region Get Order Statuses
        [HttpGet("GetOrderStatus")]
        public async Task<IActionResult> GetOrderStatus(string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var orderStatuses = await _orderStatusService.GetOrderStatusesAsync(search, sortingParams);
                return Ok(orderStatuses);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Order Status
        [HttpPost("AddOrderStatus")]
        public async Task<ActionResult> AddOrderStatus(AddOrderStatusDto addOrderStatusDto)
        {
            try
            {
                var flag = await _orderStatusService.AddOrderStatusAsync(addOrderStatusDto);
                return flag != 0 ? Ok(new { Message = "Order Status added successfully." }) : BadRequest(new { Message = "Unable to add order status." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Order Status
        [HttpPut("UpdateOrderStatus")]
        public async Task<ActionResult> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            try
            {
                var flag = await _orderStatusService.UpdateOrderStatusAsync(updateOrderStatusDto);
                return flag != 0 ? Ok(new { Message = "Order Status updated successfully." }) : BadRequest(new { Message = "Unable to update order status." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region De-Activate Order Status
        [HttpDelete("DeActivateOrderStatus")]
        public async Task<ActionResult> DeActivateOrderStatus(int id)
        {
            try
            {
                var flag = await _orderStatusService.DeActivateOrderStatusAsync(id);
                return flag != 0 ? Ok(new { Message = "Order Status deleted successfully." }) : BadRequest(new { Message = "Unable to delete order status." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
