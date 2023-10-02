using CsvHelper.Configuration.Attributes;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module
{
    public class AddAmfiSchemeDto
    {
        [Name("AMC")]
        public string? Amc { get; set; }
        [Name("Code")]
        public string? SchemeCode { get; set; }
        [Name("Scheme Name")]
        public string? SchemeName { get; set; }
        [Name("Scheme Type")]
        public string? SchemeType { get; set; }
        [Name("Scheme Category")]
        public string? SchemeCategory { get; set; }
        [Name("Scheme NAV Name")]
        public string? SchemeNavname { get; set; } 
        [Name("Scheme Minimum Amount")]
        public string? SchemeMinAmt { get; set; }
        [Name("Launch Date")]
        public DateTime? LaunchDate { get; set; }
        [Name(" Closure Date")]
        public DateTime? ClosureDate { get; set; }
        [Name("ISIN Div Payout/ ISIN GrowthISIN Div Reinvestment")]
        public string? Isin { get; set; }
    }
}
