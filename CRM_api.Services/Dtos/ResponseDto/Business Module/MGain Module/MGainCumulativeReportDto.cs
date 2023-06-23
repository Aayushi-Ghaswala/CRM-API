namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainCumulativeReportDto
    {
        public List<MgainCumulativeInterestDto> CumulativeInterests { get; set; }
        public decimal? TotalInterestForPeriod { get; set; }
        public decimal? TotalFinalAmount { get; set; }
    }
}
