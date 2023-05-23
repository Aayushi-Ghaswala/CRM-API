using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblLoanMaster
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LoanTypeId { get; set; }
        public int? BankId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Term { get; set; }
        public string? Frequency { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal? RateOfInterest { get; set; }
        public long? LoanAccountNo { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsEmailReminder { get; set; }
        public bool? IsSmsReminder { get; set; }
        public bool? IsNotification { get; set; }
        public bool? IsSendForReview { get; set; }
        public bool? IsKathrough { get; set; }
        public bool? IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual TblUserMaster TblUserMaster { get; set; }
        [ForeignKey(nameof(LoanTypeId))]
        public virtual TblLoanTypeMaster TblLoanTypeMaster { get; set; }
        [ForeignKey(nameof(BankId))]
        public virtual TblBankMaster TblBankMaster { get; set; }

    }
}
