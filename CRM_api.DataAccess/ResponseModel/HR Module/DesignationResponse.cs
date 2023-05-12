using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;

namespace CRM_api.DataAccess.ResponseModel.HR_Module
{
    public class DesignationResponse
    {
        public List<TblDesignationMaster> Values = new List<TblDesignationMaster>();

        public Pagination Pagination = new Pagination();
    }
}
