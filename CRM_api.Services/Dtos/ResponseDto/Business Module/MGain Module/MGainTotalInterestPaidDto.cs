using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainTotalInterestPaidDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? TotalInterestPaid { get; set; }
    }
}
