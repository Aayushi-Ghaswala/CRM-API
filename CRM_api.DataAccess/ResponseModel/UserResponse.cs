using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.Repositories;

namespace CRM_api.DataAccess.ResponseModel
{
    public class UserResponse
    {
        public List<TblUserMaster> Values = new List<TblUserMaster>();

        public Pagination Pagination = new Pagination();
    }
}
