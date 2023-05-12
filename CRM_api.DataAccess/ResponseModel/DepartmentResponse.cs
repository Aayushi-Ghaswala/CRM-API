using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.ResponseModel
{
    public class DepartmentResponse
    {
        public List<TblDepartmentMaster> Values = new List<TblDepartmentMaster>();

        public Pagination Pagination = new Pagination();
    }
}
