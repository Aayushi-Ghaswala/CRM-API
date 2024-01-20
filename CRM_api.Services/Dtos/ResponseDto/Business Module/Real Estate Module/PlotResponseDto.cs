using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module
{
    public class PlotResponseDto<T>
    {
        public ResponseDto<T>? response { get; set; }
        public decimal? totalPlots { get; set; }
        public decimal? totalAllocatedPlots { get; set; }
        public decimal? totalUnallocatedPlots { get; set; }
    }
}
