using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Loan_Module
{
    public class LoanMasterDto
    {
        public int Id { get; set; }
        public UserNameDto TblUserMaster { get; set; }
        public LoanTypeMasterDto TblLoanTypeMaster { get; set; }
        public BankMasterDto TblBankMaster { get; set; }
        public string? LoanAgainstProperty { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Term { get; set; }
        public string? Frequency { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal? RateOfInterest { get; set; }
        public Int64? LoanAccountNo { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsEmailReminder { get; set; }
        public bool? IsSmsReminder { get; set; }
        public bool? IsNotification { get; set; }
        public bool? IsSendForReview { get; set; }
        public bool? IsKathrough { get; set; }
        public bool? IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
