using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDepartmentRepository
    {
        Task<Response<TblDepartmentMaster>> GetDepartments(string search, SortingParams sortingParams);
        Task<int> AddDepartment(TblDepartmentMaster departmentMaster);
        Task<int> UpdateDepartment(TblDepartmentMaster departmentMaster);
        Task<int> DeactivateDepartment(int id);
    }
}
