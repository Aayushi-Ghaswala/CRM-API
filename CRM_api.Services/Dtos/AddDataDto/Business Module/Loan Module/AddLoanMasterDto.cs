namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module
{
    public class AddLoanMasterDto
    {
        public int? UserId { get; set; }
        public int? CatId { get; set; }
        public int? BankId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Term { get; set; }
        public string? Frequency { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal? RateOfInterest { get; set; }
        public Int64? LoanAccountNo { get; set; }
        public Boolean IsNotification { get; set; }
        public Boolean IsCompleted { get; set; }
    }
}
