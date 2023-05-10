using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblWbcMallProduct
    {
        public TblWbcMallProduct()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
            TblProductImgs = new HashSet<TblProductImg>();
        }

        public int ProdId { get; set; }
        public string ProdName { get; set; } = null!;
        public int ProdCatId { get; set; }
        public DateTime ProdDateAdded { get; set; }
        public int? ProdDiscount { get; set; }
        public string? ProdImage { get; set; }
        public decimal? ProdRating { get; set; }
        public string? Description { get; set; }
        public int? ProdAvailableQty { get; set; }

        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual ICollection<TblProductImg> TblProductImgs { get; set; }
    }
}
