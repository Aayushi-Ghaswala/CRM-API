namespace CRM_api.DataAccess.ResponseModel.Generic_Response
{
    public class LedgerResponse<T>
    {
        public Response<T> response { get; set; }
        public decimal? TotalCredit { get; set; }
        public decimal? TotalDebit { get; set; }
    }
}
