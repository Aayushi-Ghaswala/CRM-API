using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module
{
    public class BussinessResponse<T>
    {
        public Response<T>? response { get; set; }
        public decimal? totalPurchaseUnit { get; set; }
        public decimal? totalAmount { get; set; }
    }
}
