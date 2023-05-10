using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
        }

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string? Zip { get; set; }
        public string Phone { get; set; } = null!;
        public decimal? ShippingCharge { get; set; }
        public decimal? Tax { get; set; }
        public DateTime? ShipDate { get; set; }
        public byte OrderStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public bool Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string? DeliverType { get; set; }

        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
