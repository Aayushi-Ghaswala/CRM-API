using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IDepartmentService
    {
        Task<int> AddDepartmentAsync(AddDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto);
        Task<DisplayDepartmentDto> GetDepartmentAsync(int page);
        Task<DepartmentDto> GetDepartmentById(int id);
    }
}
