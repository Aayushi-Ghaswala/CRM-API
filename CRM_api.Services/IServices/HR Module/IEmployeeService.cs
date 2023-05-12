using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IEmployeeService
    {
        Task<int> AddEmployeeAsync(AddUserMasterDto addUser);
        Task<int> UpdateEmployeeAsync(UpdateUserMasterDto updateUser);
        Task<DisplayUserMasterDto> GetEmployeesAsync(int page);
        Task<UserMasterDto> GetEmployeeById(int id);
    }
}
