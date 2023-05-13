using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IDepartmentRepository
    {
        Task<Response<TblDepartmentMaster>> GetDepartments(int page);
        Task<int> AddDepartment(TblDepartmentMaster departmentMaster);
        Task<int> UpdateDepartment(TblDepartmentMaster departmentMaster);
        Task<TblDepartmentMaster> GetDepartmentById(int id);
    }
}
