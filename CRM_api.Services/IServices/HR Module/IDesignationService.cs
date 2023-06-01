using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.IServices.HR_Module
{
    public interface IDesignationService
    {
        Task<ResponseDto<DesignationDto>> GetDesignationAsync(string search, SortingParams sortingParams);
        Task<DesignationDto> GetDesignationByIdAsync(int id);
        Task<IEnumerable<DesignationDto>> GetDesignationByDepartmentAsync(int departmentId);
        Task<int> AddDesignationAsync(AddDesignationDto designationMaster);
        Task<int> UpdateDesignationAsync(UpdateDesignationDto designationMaster);
        Task<int> DeactivateDesignationAsync(int id);
    }
}
