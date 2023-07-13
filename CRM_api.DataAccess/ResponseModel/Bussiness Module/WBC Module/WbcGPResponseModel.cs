namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module
{
    public class WbcGPResponseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int WbcSchemeId { get; set; }
        public string WbcTypeName { get; set; }
        public string SubInvestmentType { get; set; }
        public string SubSubInvestmentType { get; set; }
        public int? ReferralGP { get; set; }
        public int? GoldPoint { get; set; }
        public int? OnTheSpotGP { get; set; } = 0;
        public bool IsRedeemable { get; set; }

        public WbcGPResponseModel()
        {
        }

        public WbcGPResponseModel(int userId, int wbcSchemeId, string wbcTypeName, string subInvestmentType, string subSubInvestmentType, int? referralGP, int? goldPoint, int? on_the_spot_GP, bool isRedeemable)
        {
            UserId = userId;
            WbcTypeName = wbcTypeName;
            SubInvestmentType = subInvestmentType;
            SubSubInvestmentType = subSubInvestmentType;
            ReferralGP = referralGP;
            GoldPoint = goldPoint;
            OnTheSpotGP = on_the_spot_GP;
            WbcSchemeId = wbcSchemeId;
            IsRedeemable = isRedeemable;
        }

        public WbcGPResponseModel(int userId, string userName, int wbcSchemeId, string wbcTypeName, string subInvestmentType, string subSubInvestmentType, int? referralGP, int? goldPoint, int? onTheSpotGP, bool isRedeemable)
        {
            UserId = userId;
            UserName = userName;
            WbcSchemeId = wbcSchemeId;
            WbcTypeName = wbcTypeName;
            SubInvestmentType = subInvestmentType;
            SubSubInvestmentType = subSubInvestmentType;
            ReferralGP = referralGP;
            GoldPoint = goldPoint;
            OnTheSpotGP = onTheSpotGP;
            IsRedeemable = isRedeemable;
        }
    }
}
