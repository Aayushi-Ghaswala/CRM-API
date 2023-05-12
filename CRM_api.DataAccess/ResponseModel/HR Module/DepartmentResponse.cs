using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;

namespace CRM_api.DataAccess.ResponseModel.HR_Module
{
    public class DepartmentResponse
    {
        public List<TblDepartmentMaster> Values = new List<TblDepartmentMaster>();

        public Pagination Pagination = new Pagination();
    }
}
