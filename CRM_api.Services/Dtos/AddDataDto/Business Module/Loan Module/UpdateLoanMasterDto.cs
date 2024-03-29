﻿namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module
{
    public class UpdateLoanMasterDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LoanTypeId { get; set; }
        public int? BankId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
    }
}
