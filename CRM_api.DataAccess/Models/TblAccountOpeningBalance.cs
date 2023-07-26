using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblAccountOpeningBalance
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? FinancialYearid { get; set; }
        public decimal? Balance { get; set; }
        public string? BalanceType { get; set; }
        public bool? Isdeleted { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual TblAccountMaster TblAccountMaster { get; set; }
        [ForeignKey(nameof(FinancialYearid))]
        public virtual TblFinancialYearMaster TblFinancialYear { get; set; }
    }
}
