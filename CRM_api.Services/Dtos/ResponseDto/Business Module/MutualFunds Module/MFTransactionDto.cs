using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class MFTransactionDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? totalBalanceUnit { get; set; }
        public decimal? totalPurchaseAmount { get; set; }
        public decimal? totalRedeemAmount { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? totalScheme { get; set;}
    }
}
