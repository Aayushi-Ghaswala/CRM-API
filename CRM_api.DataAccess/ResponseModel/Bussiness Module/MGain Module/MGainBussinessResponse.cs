using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module
{
    public class MGainBussinessResponse<T>
    {
        public Response<T>? response { get; set; }
        public decimal? totalAmount { get; set; }
        public decimal? redemAmount { get; set; }
        public decimal? remainingAmount { get; set; }
        public int? totalMGain { get; set; }
    }
}
