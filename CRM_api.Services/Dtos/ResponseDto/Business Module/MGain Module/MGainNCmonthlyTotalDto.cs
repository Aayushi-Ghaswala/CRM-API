namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainNCmonthlyTotalDto
    {
        public List<MGainNonCumulativeMonthlyReportDto> MGainNonCumulativeMonthlyReports { get; set; }
        public decimal? TotalInterestAmount { get; set; }
        public decimal? TotalTDSAmount { get; set; }
        public decimal? TotalPayAmount { get; set; }
        public int? TotalMGain { get; set; }
    }
}
