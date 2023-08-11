using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IEmployeeService
    {
        Task<ResponseDto<EmployeeMasterDto>> GetEmployeesAsync(string search, SortingParams sortingParams);
        Task<(int, string)> AddEmployeeAsync(AddEmployeeDto addEmployee);
        Task<int> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployee);
        Task<int> DeactivateEmployeeAsync(int id);
        Task<int> DeleteEmployeeQualificationAsync(int id);
        Task<int> DeleteEmployeeExperienceAsync(int id);
    }
}
