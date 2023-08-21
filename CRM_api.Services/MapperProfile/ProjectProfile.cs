using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<TblProjectMaster, ProjectMasterDto>();
            CreateMap<Response<TblProjectMaster>, ResponseDto<ProjectMasterDto>>();
            CreateMap<TblProjectTypeDetail, ProjectTypeDetailDto>();
            CreateMap<AddProjectDto, TblProjectMaster>();
            CreateMap<UpdateProjectDto, TblProjectMaster>();
        }
    }
}
