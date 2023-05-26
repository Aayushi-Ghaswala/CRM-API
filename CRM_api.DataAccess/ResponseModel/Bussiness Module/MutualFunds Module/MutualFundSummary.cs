namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module
{
    public class MutualFundSummary
    {
        public string? Schemename { get; set; }
        public string? Foliono { get; set; }
        public decimal? TotalPurchaseUnit { get; set; }
        public decimal? TotalRedemptionUnit { get; set; }
        public decimal? BalanceUnit { get; set; }
        public double? NAV { get; set; }
        public decimal? CurrentValue { get; set; }
    }
}
