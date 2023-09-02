using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.HR_Module
{
    public interface IEmployeeRepository
    {
        Task<Response<TblEmployeeMaster>> GetEmployees(string search, SortingParams sortingParams);
        Task<TblEmployeeMaster> GetEmployeeByName(string name);
        Task<TblEmployeeMaster> AddEmployee(TblEmployeeMaster employeeMaster);
        Task<int> AddEmployeeQualification(TblEmployeeQualification employeeQualification);
        Task<int> AddEmployeeExperience(TblEmployeeExperience employeeExperience);
        Task<int> UpdateEmployee(TblEmployeeMaster employeeMaster);
        Task<int> UpdateEmployeeQualification(TblEmployeeQualification employeeQualification);
        Task<int> UpdateEmployeeExperience(TblEmployeeExperience employeeExperience);
        Task<int> DeactivateEmployee(int id);
        Task<int> DeleteEmployeeQualification(int id);
        Task<int> DeleteEmployeeExperience(int id);
    }
}
