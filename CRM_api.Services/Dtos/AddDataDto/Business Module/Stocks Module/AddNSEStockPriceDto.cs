namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddNSEStockPriceDto
    {
        public string? SYMBOL { get; set; }
        public string? SERIES { get; set; }
        public decimal? OPEN { get; set; }
        public decimal? HIGH { get; set; }
        public decimal? LOW { get; set; }
        public decimal? CLOSE { get; set; }
        public decimal? LAST { get; set; }
        public decimal? PREVCLOSE { get; set; }
        public decimal? TOTTRDQTY { get; set; }
        public decimal? TOTTRDVAL { get; set; }
        public DateTime? TIMESTAMP { get; set; }
        public int? TOTALTRADES { get; set; }
        public string? ISIN { get; set; }
    }
}
