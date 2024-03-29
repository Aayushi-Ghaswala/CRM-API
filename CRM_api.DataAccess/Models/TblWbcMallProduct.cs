﻿using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal? ProdPrice { get; set; }
        public decimal? GoldPointPrice { get; set; }
        public string? ManagedBy { get; set; }
        public bool? IsShowInApp { get; set; }

        [ForeignKey(nameof(ProdCatId))]
        public virtual TblWbcMallCategory TblWbcMallCategory { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual ICollection<TblProductImg> TblProductImgs { get; set; }
    }
}
