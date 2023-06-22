using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblAccountMaster
    {
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public int? UserId { get; set; }
        public double? OpeningBalance { get; set; }
        public string? DebitCredit { get; set; }
        public DateTime? OpeningBalanceDate { get; set; }
    }
}
