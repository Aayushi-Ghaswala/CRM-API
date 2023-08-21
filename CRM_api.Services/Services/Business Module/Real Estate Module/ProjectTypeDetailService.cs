using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;

namespace CRM_api.Services.Services.Business_Module.Real_Estate_Module
{
    public class ProjectTypeDetailService : IProjectTypeDetailService
    {
        private readonly IProjectTypeDetailRepository _projectTypeDetailRepository;
        private readonly IMapper _mapper;

        public ProjectTypeDetailService(IProjectTypeDetailRepository projectTypeDetailRepository, IMapper mapper)
        {
            _projectTypeDetailRepository = projectTypeDetailRepository;
            _mapper = mapper;
        }

        #region Get Project Type Details
        public async Task<ResponseDto<ProjectTypeDetailDto>> GetProjectTypeDetailsAsync(int? projectTypeId, string? search, SortingParams sortingParams)
        {
            var projectTypeDetails = await _projectTypeDetailRepository.GetProjectTypeDetails(projectTypeId, search, sortingParams);
            var mappedProjectTypeDetails = _mapper.Map<ResponseDto<ProjectTypeDetailDto>>(projectTypeDetails);

            return mappedProjectTypeDetails;
        }
        #endregion

        #region Get Project Types
        public async Task<ResponseDto<ProjectTypeDto>> GetProjectTypesAsync(string? search, SortingParams sortingParams)
        {
            var projectTypes = await _projectTypeDetailRepository.GetProjectTypes(search, sortingParams);
            var mappedProjectTypes = _mapper.Map<ResponseDto<ProjectTypeDto>>(projectTypes);

            return mappedProjectTypes;
        }
        #endregion

        #region Add Project Type Detail
        public async Task<int> AddProjectTypeDetailAsync(AddProjectTypeDetailDto addProjectTypeDetailDto)
        {
            var projectTypeDetail = _mapper.Map<TblProjectTypeDetail>(addProjectTypeDetailDto);
            return await _projectTypeDetailRepository.AddProjectTypeDetail(projectTypeDetail);
        }
        #endregion

        #region Update Project Type Detail
        public async Task<int> UpdateProjectTypeDetailAsync(UpdateProjectTypeDetailDto updateProjectTypeDetailDto)
        {
            var projectTypeDetail = _mapper.Map<TblProjectTypeDetail>(updateProjectTypeDetailDto);
            return await _projectTypeDetailRepository.UpdateProjectTypeDetail(projectTypeDetail);
        }
        #endregion

        #region Delete Project Type Detail
        public async Task<int> DeleteProjectTypeDetailAsync(int id)
        {
            return await _projectTypeDetailRepository.DeleteProjectTypeDetail(id);
        }
        #endregion
    }
}
