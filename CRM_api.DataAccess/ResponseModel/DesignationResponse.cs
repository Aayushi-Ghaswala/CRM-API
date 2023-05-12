using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.ResponseModel
{
    public class DesignationResponse
    {
        public List<TblDesignationMaster> Values = new List<TblDesignationMaster>();

        public Pagination Pagination = new Pagination();
    }
}
