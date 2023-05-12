namespace CRM_api.DataAccess.ResponseModel.User_Module
{
    public class Response<T>
    {
        public List<T> Values { get; set; }
        public Pagination Pagination { get; set; }
    }
}
