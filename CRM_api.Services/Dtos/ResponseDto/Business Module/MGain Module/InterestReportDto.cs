namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class InterestReportDto
    {
        public int DepositeCode { get; set; }
        public DateTime? Date { get; set; }
        public string ShcemeName { get; set; }
        public string ACDescription { get; set; }
        public decimal? InterestPaid { get; set; }
        public decimal? InterestAccrued { get; set; }
        public decimal? TaxDeducted { get; set; }
        public string? OverheadTaxDeducted { get; set; }
        public string Currency { get; set; }
    }
}
