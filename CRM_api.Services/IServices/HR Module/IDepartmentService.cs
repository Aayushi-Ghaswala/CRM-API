using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IDepartmentService
    {
        Task<ResponseDto<DepartmentDto>> GetDepartmentAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<DepartmentDto> GetDepartmentById(int id);
        Task<int> AddDepartmentAsync(AddDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdateDepartmentDto departmentDto);
        Task<int> DeactivateDepartmentAsync(int id);
    }
}
