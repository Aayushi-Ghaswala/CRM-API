using CsvHelper.Configuration.Attributes;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module
{
    public class AddNJDailyPriceDto
    {
        [Name("Sr.No")]
        public int SrNo { get; set; }
        [Name("Scheme")]
        public string? SchemeName { get; set; }
        [Name("NAV Date")]
        public DateTime? SchemeDate { get; set; }
        [Name("NAV")]
        public string? NetAssetValue { get; set; }
        [Name("% Change")]
        public string Changes { get; set; }
    }
}
