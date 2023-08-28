using CsvHelper.Configuration.Attributes;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddBSEStockPriceDto
    {
        [Name("SC_CODE")]
        public string? SCCode { get; set; }
        [Name("SC_NAME")]
        public string? SCName { get; set; }
        [Name("SC_GROUP")]
        public string? SCGroup { get; set; }
        [Name("SC_TYPE")]
        public string? SCType { get; set; }
        public decimal? OPEN { get; set; }
        public decimal? HIGH { get; set; }
        public decimal? LOW { get; set; }
        public decimal? CLOSE { get; set; }
        public decimal? LAST { get; set; }
        public decimal? PREVCLOSE { get; set; }
        public decimal? NO_TRADES { get; set; }
        public decimal? NO_OF_SHRS { get; set; }
        public decimal? NET_TURNOV { get; set; }
        public string? TDCLOINDI { get; set; }
    }
}
