using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Real_Estate_Module
{
    public interface IProjectTypeDetailService
    {
        Task<ResponseDto<ProjectTypeDetailDto>> GetProjectTypeDetailsAsync(int? projectTypeId, string? search, SortingParams sortingParams);
        Task<ResponseDto<ProjectTypeDto>> GetProjectTypesAsync(string? search, SortingParams sortingParams);
        Task<int> AddProjectTypeDetailAsync(AddProjectTypeDetailDto addProjectTypeDetailDto);
        Task<int> UpdateProjectTypeDetailAsync(UpdateProjectTypeDetailDto updateProjectTypeDetailDto);
        Task<int> DeleteProjectTypeDetailAsync(int id);
    }
}
