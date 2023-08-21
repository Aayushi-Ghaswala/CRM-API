using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;

namespace CRM_api.Services.Services.Business_Module.Real_Estate_Module
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository ProjectRepository, IMapper mapper, IRegionRepository regionRepository)
        {
            _ProjectRepository = ProjectRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
        }

        #region Get All Project
        public async Task<ResponseDto<ProjectMasterDto>> GetProjectAsync(bool? isActive, string? search, SortingParams sortingParams)
        {
            var Projects = await _ProjectRepository.GetProjects(isActive, search, sortingParams);
            var mapProject = _mapper.Map<ResponseDto<ProjectMasterDto>>(Projects);

            foreach (var project in mapProject.Values)
            {
                if (project.State is not null)
                {
                    var state = await _regionRepository.GetStateByName(project.State);
                    project.StateMaster = _mapper.Map<StateMasterDto>(state);
                }
                if (project.City is not null)
                {
                    var city = await _regionRepository.GetCityByName(project.City);
                    project.CityMaster = _mapper.Map<CityMasterDto>(city);
                }
            }

            return mapProject;
        }
        #endregion

        #region Add Project
        public async Task<int> AddProjectAsync(AddProjectDto addProject)
        {
            var mapProject = _mapper.Map<TblProjectMaster>(addProject);
            mapProject.IsActive = true;
            return await _ProjectRepository.AddProject(mapProject);
        }
        #endregion

        #region Update Project
        public async Task<int> UpdateProjectAsync(UpdateProjectDto updateProject)
        {
            var mapProject = _mapper.Map<TblProjectMaster>(updateProject);
            return await _ProjectRepository.UpdateProject(mapProject);
        }
        #endregion

        #region Deactivate Project
        public async Task<int> DeactivateProjectAsync(int id)
        {
            return await _ProjectRepository.DeactivateProject(id);
        }
        #endregion
    }
}
