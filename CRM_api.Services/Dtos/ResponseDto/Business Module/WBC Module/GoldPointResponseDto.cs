using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class GoldPointResponseDto<T>
    {
        public ResponseDto<T> response { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? TotalDebit { get; set; }
    }
}
