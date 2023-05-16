using CsvHelper.Configuration.Attributes;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddSharekhanStocksDto
    {
        [Name("Branch")]
        public int StBranch { get; set; }
        [Name("Client Code")]
        public int StClientcode { get; set; }
        [Name("Client Name")]
        public string? StClientname { get; set; }
        [Name("Scrip Name")]
        public string? StScripname { get; set; }
        [Name("Settlement No")]
        public string? StSettno { get; set; }
        [Name("Date")]
        public DateTime? StDate { get; set; }
        [Name("Buy/Sel")]
        public string? StType { get; set; }
        [Name("Quantity")]
        public int? StQty { get; set; }
        [Name("Rate")]
        public decimal? StRate { get; set; }
        [Name("Brokerage")]
        public decimal? StBrokerage { get; set; }
        [Name("Net Rate")]
        public decimal? StNetrate { get; set; }
        [Name("Net Value")]
        public decimal? StNetvalue { get; set; }
        [Name("Cost Per Share")]
        public decimal? StCostpershare { get; set; }
        [Name("Cost Value")]
        public decimal? StCostvalue { get; set; }
        [Name("Net Share Rate")]
        public decimal? StNetsharerate { get; set; }
        [Name("Net Cost Value")]
        public decimal? StNetcostvalue { get; set; }
        //[Name("")]
        //public int? Userid { get; set; } = null;
    }
}
