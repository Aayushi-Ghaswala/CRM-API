using CsvHelper.Configuration.Attributes;
using System.ComponentModel;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module
{
    public class AddLoanMasterDto
    {
        public int? UserId { get; set; }
        public int? LoanTypeId { get; set; }
        public int? BankId { get; set; }
        public string? LoanAgainstProperty { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Term { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal? RateOfInterest { get; set; }
        public long? LoanAccountNo { get; set; }
        [DefaultValue(false)]
        public bool? IsEmailReminder { get; set; }
        [DefaultValue(false)]
        public bool? IsSmsReminder { get; set; }
        [DefaultValue(false)]
        public bool? IsNotification { get; set; }
        [DefaultValue(false)]
        public bool? IsSendForReview { get; set; }
        [DefaultValue(false)]
        public bool? IsKathrough { get; set; }
        [DefaultValue(false)]
        public bool? IsCompleted { get; set; }
    }
}
