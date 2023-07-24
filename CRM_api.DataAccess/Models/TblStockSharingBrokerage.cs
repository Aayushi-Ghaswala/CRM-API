namespace CRM_api.DataAccess.Models
{
    public partial class TblStockSharingBrokerage
    {
        public int Id { get; set; }
        public string BrokerageName { get; set; }
        public decimal BrokeragePercentage { get; set; }
    }
}