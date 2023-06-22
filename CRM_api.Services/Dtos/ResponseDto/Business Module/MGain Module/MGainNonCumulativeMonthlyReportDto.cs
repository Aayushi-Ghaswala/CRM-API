using Org.BouncyCastle.Math;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainNonCumulativeMonthlyReportDto
    {
        public long? IntAccNo { get; set; }
        public string? IntBankName { get; set; }
        public string? MgainRedemdate { get; set; }
        public string? Date { get; set; }
        public string? Mgain1stholder { get; set; }
        public decimal? MgainInvamt { get; set; }
        public string? MgainType { get; set; }
        public bool? YearlyInterest { get; set; }
        public bool? MonthlyInterest { get; set; }
        public decimal? Interst1 { get; set; }
        public decimal? Interst4 { get; set; }
        public decimal? Interst8 { get; set; }
        public decimal? InterestAmount { get; set; }
        public decimal? TDS { get; set; }
        public decimal? PayAmount { get; set; }
        public decimal? InterstRate { get; set; }
    }
}
