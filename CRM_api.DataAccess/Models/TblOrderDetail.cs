using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblOrderDetail
    {
        public int Id { get; set; }
        public int OderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Sku { get; set; } = null!;
        public int Quantity { get; set; }

        public virtual TblOrder Oder { get; set; } = null!;
        public virtual TblWbcMallProduct Product { get; set; } = null!;
    }
}
