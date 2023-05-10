using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMgainTransactionAccountTem
    {
        public int Id { get; set; }
        public DateTime? MgainDate { get; set; }
        public string? MgainParticular { get; set; }
        public string? MgainVchtype { get; set; }
        public int? MgainVchno { get; set; }
        public decimal? MgainDebit { get; set; }
        public decimal? MgainCredit { get; set; }
        public string? MgainType { get; set; }
        public int? MgainUserid { get; set; }
        public int? Accountid { get; set; }
    }
}
