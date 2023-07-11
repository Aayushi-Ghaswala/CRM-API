namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class WbcGPResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string WbcTypeName { get; set; }
        public string SubInvestmentType { get; set; }
        public string SubSubInvestmentType { get; set; }
        public int? ReferralGP { get; set; }
        public int? GoldPoint { get; set; }
        public int? OnTheSpotGP { get; set; } = 0;
        public bool IsRedeemable { get; set; }
    }
}
