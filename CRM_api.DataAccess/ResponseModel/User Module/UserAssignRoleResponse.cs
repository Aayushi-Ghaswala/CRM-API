using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.Repositories;

namespace CRM_api.DataAccess.ResponseModel.User_Module
{
    public class UserAssignRoleResponse
    {
        public List<TblRoleAssignment> Values { get; set; }
        public Pagination Pagination { get; set; }
    }
}
