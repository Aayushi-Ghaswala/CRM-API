using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.ResponseModel.Stocks_Module
{
    public class StocksResponse<T>
    {
        public Response<T> response { get; set; }
        public decimal? TotalPurchase { get; set; } = 0;
        //public decimal? TotalPurchaseQty { get; set; } = 0;
        public decimal? TotalSale { get; set; } = 0;
        //public decimal? TotalSaleQty { get; set; } = 0;
    }
}
