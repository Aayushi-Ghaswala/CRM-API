using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class ProjectTypeDetailProfile : Profile
    {
        public ProjectTypeDetailProfile()
        {
            CreateMap<TblProjectTypeDetail, ProjectTypeDetailDto>();
            CreateMap<Response<TblProjectTypeDetail>, ResponseDto<ProjectTypeDetailDto>>();
            CreateMap<TblProjectTypeMaster, ProjectTypeDto>();
            CreateMap<Response<TblProjectTypeMaster>, ResponseDto<ProjectTypeDto>>();
            CreateMap<AddProjectTypeDetailDto, TblProjectTypeDetail>();
            CreateMap<UpdateProjectTypeDetailDto, TblProjectTypeDetail>();
        }
    }
}
