using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblGoldPoint
    {
        public int GpId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Credit { get; set; }
        public int? Debit { get; set; }
        public int? Userid { get; set; }
        public string? Type { get; set; }
        public int? PointCategory { get; set; }
        public int? Vendorid { get; set; }

        public virtual TblGoldPointCategory? PointCategoryNavigation { get; set; }
    }
}
