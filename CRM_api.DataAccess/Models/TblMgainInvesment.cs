using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMgainInvesment
    {
        public int MgainId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public decimal Rate { get; set; }
        public DateTime InvestmentDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public bool IsActive { get; set; }

        public virtual TblUserMaster User { get; set; } = null!;
    }
}
