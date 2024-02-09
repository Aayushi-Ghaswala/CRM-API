namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module
{
    public class AddMgainRedemptionRequestsDto
    {
        public int MgainId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public DateTime? RecordDate { get; set; }
        public decimal? NoOfDays { get; set; }
        public decimal? RedemptionAmount { get; set; }
        public decimal? TotalInterest { get; set; }
        public decimal? MaturityAmount { get; set; }
        public decimal? AmountDeducted { get; set; }
        public decimal First3MonthInterest { get; set; }
        public string? RedemptionDoc { get; set; }
        public int AccountId { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool? IsCompleted { get; set; }
        public string? Reason { get; set; }
        public decimal? DaysInterest { get; set; }
    }
}
