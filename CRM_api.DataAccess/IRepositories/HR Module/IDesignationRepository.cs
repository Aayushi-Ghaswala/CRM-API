using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDesignationRepository
    {
        Task<DesignationResponse> GetDesignation(int page);
        Task<int> AddDesignatione(TblDesignationMaster designationMaster);
        Task<int> UpdateDesignation(TblDesignationMaster designationMaster);
        Task<TblDesignationMaster> GetDesignationById(int id);
        Task<IEnumerable<TblDesignationMaster>> GetDesignationByDepartment(int deptId);
    }
}
