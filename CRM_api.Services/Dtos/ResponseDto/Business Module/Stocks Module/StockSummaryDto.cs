using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module
{
    public class StockSummaryDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? totalBalanceQuantity { get; set; }
        public decimal? totalAmount { get; set; }
    }
}
