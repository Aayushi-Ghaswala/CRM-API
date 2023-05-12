using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.ResponseModel.User_Module
{
    public class UserResponse
    {
        public List<TblUserMaster> Values = new List<TblUserMaster>();

        public Pagination Pagination = new Pagination();
    }
}
