namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module
{
    public class StocksDashboardSummaryDto
    {
        public string Duration { get; set; }
        public int Clients { get; set; }
        public decimal? TotalAmount { get; set; }
        public int Delivery { get; set; }
        public decimal? DeliveryAmount { get; set; }
        public int IntraDay { get; set; }
        public decimal? IntraDayAmount { get; set; }

        public StocksDashboardSummaryDto(string duration, int clients, decimal totalAmount, int delivery, decimal? deliveryAmount, int intraDay, decimal? intraDayAmount)
        {
            Duration = duration;
            Clients = clients;
            TotalAmount = totalAmount;
            Delivery = delivery;
            DeliveryAmount = deliveryAmount;
            IntraDay = intraDay;
            IntraDayAmount = intraDayAmount;
        }
    }
}
