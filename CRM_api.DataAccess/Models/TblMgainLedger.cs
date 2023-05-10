using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMgainLedger
    {
        public int LedgerId { get; set; }
        public int MgainId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime InvestmentDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public bool Isdeleted { get; set; }
    }
}
