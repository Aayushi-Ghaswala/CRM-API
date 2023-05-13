using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDesignationRepository
    {
        Task<Response<TblDesignationMaster>> GetDesignation(int page);
        Task<int> AddDesignation(TblDesignationMaster designationMaster);
        Task<int> UpdateDesignation(TblDesignationMaster designationMaster);
        Task<TblDesignationMaster> GetDesignationById(int id);
        Task<IEnumerable<TblDesignationMaster>> GetDesignationByDepartment(int deptId);
    }
}
