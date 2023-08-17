using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Crmf;

namespace CRM_api.Controllers.WBC_Mall_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Get Orders
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders(int? statusId, string? search, [FromQuery] SortingParams sortingParams)
        {
            try
            {
                var orders = await _orderService.GetOrderAsync(statusId, search, sortingParams);
                return Ok(orders);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Order
        [HttpPut("UpdateOrder")]
        public async Task<ActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            try
            {
                var flag = await _orderService.UpdateOrderAsync(updateOrderDto);
                return flag.Item2 != 0 ? Ok(new { Message = flag.Item1 }) : BadRequest(new { Message = flag.Item1 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
