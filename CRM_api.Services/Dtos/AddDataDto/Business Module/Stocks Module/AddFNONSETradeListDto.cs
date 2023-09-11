using CsvHelper.Configuration.Attributes;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddFNONSETradeListDto
    {
        [Name("Branch")]
        public string StBranch { get; set; }
        [Name("Client Code")]
        public string StClientcode { get; set; }
        [Name("Client Name")]
        public string? StClientname { get; set; }
        [Name("Series")]
        public string? StScripname { get; set; }
        public string? Type { get; set; }
        [Name("Terminal ID")]
        public string? TerminalID { get; set; }
        [Name("Date")]
        public DateTime? StDate { get; set; }
        [Name("Trxn Type")]
        public string? TrxnType { get; set; }
        [Name("B/S")]
        public string? StType { get; set; }
        [Name("Qty")]
        public int? StQty { get; set; }
        [Name("Rate Prem")]
        public decimal? StNetsharerate { get; set; }
        [Name("Srtike Price")]
        public decimal? StrikePrice { get; set; }
        [Name("Brok %")]
        public decimal? brokerage { get; set; }
        [Name("Brok Per Share")]
        public decimal? StBrokerage { get; set; }
        public decimal? Brok { get; set; }
        [Name("Service Tax")]
        public decimal? ServiceTax { get; set; }
        [Name("SB Cess")]
        public decimal? SBCess { get; set; }
        [Name("KK Cess")]
        public decimal? KKCess { get; set; }
        public decimal? IGST { get; set; }
        public decimal? SGST { get; set; }
        public decimal? CGST { get; set; }
        public decimal? UTGST { get; set; }
        [Name("Market Lot")]
        public decimal? MarketLot { get; set; }
        [Name("Educ Cess")]
        public decimal? EducCess { get; set; }
        [Name("Higher Educ Cess")]
        public decimal? HigherEducCess { get; set; }
        [Name("Stamp Charge")]
        public decimal? StampCharge { get; set; }
        [Name("To Charge")]
        public decimal? ToCharge { get; set; }
        public decimal? SebiFees { get; set; }
        public decimal? STT { get; set; }
        [Name("Net Amount")]
        public decimal? StNetcostvalue { get; set; }
        public decimal? Cess { get; set; }
        public string? Exchange { get; set; }
    }
}
