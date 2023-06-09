using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainResponseDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? redemAmount { get; set; }
        public decimal? remainingAmount { get; set; }
    }
}
