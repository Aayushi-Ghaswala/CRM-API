namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MgainCumulativeInterestDto
    {
        public DateTime? Date { get; set; }
        public int MGainId { get; set; }
        public string? ClientName { get; set; }
        public string? SchemeName { get; set; }
        public decimal InvestmentAmount { get; set; } = 0;
        public decimal InterestForPeriod { get; set; } = 0;
        public decimal FinalAmount { get; set; } = 0;
    }
}
