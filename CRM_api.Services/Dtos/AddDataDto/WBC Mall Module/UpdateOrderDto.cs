using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        //public int UserId { get; set; }
        //public decimal Amount { get; set; }
        //public string? ShipName { get; set; }
        //public string? ShipAddress { get; set; }
        //public int CountryId { get; set; }
        //public int StateId { get; set; }
        //public int CityId { get; set; }
        //public string? Zip { get; set; }
        //public string Phone { get; set; } = null!;
        //public decimal? ShippingCharge { get; set; }
        //public decimal? Tax { get; set; }
        //public DateTime? ShipDate { get; set; }
        //public byte OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
        //public bool Status { get; set; }
        //public DateTime OrderDate { get; set; }
        //public string? DeliverType { get; set; }
        //public int? OrderStatusId { get; set; }
        public OrderStatusDto TblOrderStatus { get; set; }
        //public DateTime? PackedDate { get; set; }
        //public DateTime? ReadyToPickupDate { get; set; }
        //public DateTime? DeleveredDate { get; set; }
        //public DateTime? CancelledDate { get; set; }
        //public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
