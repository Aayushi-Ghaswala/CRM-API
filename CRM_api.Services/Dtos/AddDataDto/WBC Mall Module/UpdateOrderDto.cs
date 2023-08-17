using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public OrderStatusDto TblOrderStatus { get; set; }
    }
}
