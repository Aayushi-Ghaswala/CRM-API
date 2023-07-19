namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module
{
    public class ScripwiseSummaryDto
    {
        public string? StClientname { get; set; }
        public string? StScripname { get; set; }
        public int? TotalBuyQuantity { get; set; }
        public int? TotalSellQuantity { get; set;}
        public int? TotalAvailableQuantity { get; set; }
        public decimal? NetCostValue { get; set; }
        public decimal? TotalCurrentValue { get; set; }
    }
}
