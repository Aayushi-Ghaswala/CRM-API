using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDepartmentRepository
    {
        Task<DepartmentResponse> GetDepartments(int page);
        Task<int> AddDepartment(TblDepartmentMaster departmentMaster);
        Task<int> UpdateDepartment(TblDepartmentMaster departmentMaster);
        Task<TblDepartmentMaster> GetDepartmentById(int id);
    }
}
