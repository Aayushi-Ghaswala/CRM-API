using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module
{
    public class GoldPointResponse<T>
    {
        public Response<T> response { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? TotalDebit { get; set; }
    }
}
