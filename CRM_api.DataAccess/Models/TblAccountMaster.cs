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
        public int? AccountGrpid { get; set; }
        public int? Companyid { get; set; }
        public string? GstNo { get; set; }
        public DateTime? GstRegDate { get; set; }
        public string? AccountMobile { get; set; }
        public string? AccountEmail { get; set; }
        public bool? Isdeleted { get; set; }

        [ForeignKey(nameof(AccountGrpid))]
        public virtual TblAccountGroupMaster TblAccountGroupMaster { get; set; }

        [ForeignKey(nameof(Companyid))]
        public virtual TblCompanyMaster TblCompanyMaster { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual TblUserMaster UserMaster { get; set; }

        public virtual ICollection<TblAccountOpeningBalance> TblAccountOpeningBalances { get; set; }
    }
}
