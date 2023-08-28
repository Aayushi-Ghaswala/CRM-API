namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddFNONSEStockPriceDto
    {
        public string? INSTRUMENT { get; set; }
        public string? SYMBOL { get; set; }
        public DateOnly? EXPIRY_DT { get; set; }
        public decimal? STRIKE_PR { get; set; }
        public string? OPTION_TYP { get; set; }
        public decimal? OPEN { get; set; }
        public decimal? HIGH { get; set; }
        public decimal? LOW { get; set; }
        public decimal? CLOSE { get; set; }
        public decimal? SETTLE_PR { get; set; }
        public decimal? CONTRACTS { get; set; }
        public decimal? VAL_INLAKH { get; set; }
        public decimal? OPEN_INT { get; set; }
        public decimal? CHG_IN_OI { get; set; }
        public DateTime? TIMESTAMP { get; set; }
    }
}
