using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblAccountTransaction
    {
        public int Id { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocParticulars { get; set; }
        public string? DocType { get; set; }
        public string? DocNo { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? DocSubType { get; set; }
        public int? DocUserid { get; set; }
        public int? Accountid { get; set; }
        public int? Mgainid { get; set; }

        [ForeignKey(nameof(Mgainid))]
        public virtual TblMgaindetail TblMgaindetail { get; set; }
    }
}