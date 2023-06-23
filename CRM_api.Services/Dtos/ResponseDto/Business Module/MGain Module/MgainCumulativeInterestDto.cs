namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MgainCumulativeInterestDto
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Mgain1stholder { get; set; }
        public string? MgainSchemename { get; set; }
        public decimal InvestmentAmount { get; set; } = 0;
        public decimal InterestForPeriod { get; set; } = 0;
        public decimal FinalAmount { get; set; } = 0;
    }
}
