using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Real_Estate_Module
{
    public interface IProjectService
    {
        Task<ResponseDto<ProjectMasterDto>> GetProjectAsync(bool? isActive, string? search, SortingParams sortingParams);
        Task<int> AddProjectAsync(AddProjectDto addProject);
        Task<int> UpdateProjectAsync(UpdateProjectDto updateProject);
        Task<int> DeactivateProjectAsync(int id);
    }
}
