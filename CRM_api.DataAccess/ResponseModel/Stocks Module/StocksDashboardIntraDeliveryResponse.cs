namespace CRM_api.DataAccess.ResponseModel.Stocks_Module
{
    public class StocksDashboardIntraDeliveryResponse
    {
        public string Duration { get; set; }
        public decimal? TotalIntradayBuy { get; set; } = 0;
        public decimal? TotalIntradaySale { get; set; } = 0;
        public decimal? TotalDeliveryBuy { get; set; } = 0;
        public decimal? TotalDeliverySale { get; set; } = 0;
        public decimal? TotalPurchase { get; set; } = 0;
        public decimal? TotalSale { get; set; } = 0;

        public StocksDashboardIntraDeliveryResponse(string duration, decimal? totalIntradayBuy, decimal? totalIntradaySale, decimal? totalDeliveryBuy, decimal? totalDeliverySale, decimal? totalPurchase, decimal? totalSale)
        {
            Duration = duration;
            TotalIntradayBuy = totalIntradayBuy;
            TotalIntradaySale = totalIntradaySale;
            TotalDeliveryBuy = totalDeliveryBuy;
            TotalDeliverySale = totalDeliverySale;
            TotalPurchase = totalPurchase;
            TotalSale = totalSale;
        }
    }
}
