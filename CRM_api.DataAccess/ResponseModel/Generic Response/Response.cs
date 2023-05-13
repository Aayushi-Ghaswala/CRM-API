namespace CRM_api.DataAccess.ResponseModel.Generic_Response
{
    public class Response<T>
    {
        public List<T> Values { get; set; }
        public Pagination Pagination { get; set; }
    }
}
