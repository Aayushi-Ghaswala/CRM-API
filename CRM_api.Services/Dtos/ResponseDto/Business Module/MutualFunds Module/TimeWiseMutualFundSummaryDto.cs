namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class TimeWiseMutualFundSummaryDto
    {
        public string? Duration { get; set; }
        public int? SIP { get; set; } = 0;
        public decimal? SIPAmount { get; set; } = 0;
        public int? LumpSump { get; set; } = 0;
        public decimal? LumpsumpAmount { get; set; } = 0;
        public int? Redeemption { get; set; } = 0;
        public decimal? RedeemptionAmount { get; set; } = 0;
    }
}
