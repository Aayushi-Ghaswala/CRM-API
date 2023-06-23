namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainTenYearReportDto
    {
        public string UserName { get; set; }
        public string MGainScheme { get; set; }
        public string MGainType { get; set; }
        public List<decimal?> InterestRates { get; set; }
        public DateTime InvDate { get; set; }
        public decimal MGainAmount { get; set; }
        public List<string> Months { get; set; }
        public List<MonthDetailDto> YearlyInterests { get; set; }
        public decimal TenYearTotalInterest { get; set; }
        public decimal TotalReturn { get; set; }
        public DateTime TotalReturnRecievedOn { get; set; }
    }
}
