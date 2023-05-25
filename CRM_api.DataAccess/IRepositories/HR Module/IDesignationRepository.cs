using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDesignationRepository
    {
        Task<Response<TblDesignationMaster>> GetDesignation(string search, SortingParams sortingParams);
        Task<TblDesignationMaster> GetDesignationById(int id);
        Task<IEnumerable<TblDesignationMaster>> GetDesignationByDepartment(int departmentId);
        Task<int> AddDesignation(TblDesignationMaster designationMaster);
        Task<int> UpdateDesignation(TblDesignationMaster designationMaster);
        Task<int> DeactivateDesignation(int id);
    }
}
