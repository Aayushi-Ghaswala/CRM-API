using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFolioDetail
    {
        public int Id { get; set; }
        public int Folioid { get; set; }
        public int ScripId { get; set; }
        public decimal Qty { get; set; }
        public decimal? BuyRate { get; set; }
        public decimal? BuyAmt { get; set; }

        public virtual TblFolioMaster Folio { get; set; } = null!;
    }
}
