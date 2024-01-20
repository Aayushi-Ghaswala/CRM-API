using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.RealEstateModule
{
    public class PlotResponse<T>
    {
        public Response<T>? response { get; set; }
        public decimal? totalPlots { get; set; }
        public decimal? totalAllocatedPlots { get; set; }
        public decimal? totalUnallocatedPlots { get; set; }
    }
}
