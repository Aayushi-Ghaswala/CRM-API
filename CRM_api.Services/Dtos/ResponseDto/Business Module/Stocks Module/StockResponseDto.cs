using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module
{
    public class StockResponseDto<T>
    {
        public ResponseDto<T> response { get; set; }
        public decimal? TotalPurchase { get; set; } = 0;
        public decimal? TotalPurchaseQty { get; set; } = 0;
        public decimal? TotalSale { get; set; } = 0;
        public decimal? TotalSaleQty { get; set; } = 0;
    }
}
