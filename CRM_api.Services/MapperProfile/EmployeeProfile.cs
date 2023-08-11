using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.MapperProfile
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<TblEmployeeMaster, EmployeeMasterDto>();
            CreateMap<Response<TblEmployeeMaster>, ResponseDto<EmployeeMasterDto>>();
            CreateMap<AddEmployeeDto, TblEmployeeMaster>();
            CreateMap<UpdateEmployeeDto, TblEmployeeMaster>();
            CreateMap<TblEmployeeExperience, EmployeeExperieneDto>();
            CreateMap<TblEmployeeQualification, EmployeeQualificationDto>();
            CreateMap<AddEmployeeExperienceDto, TblEmployeeExperience>();
            CreateMap<AddEmployeeQualificationDto, TblEmployeeQualification>();
            CreateMap<UpdateEmployeeExperienceDto, TblEmployeeExperience>();
            CreateMap<UpdateEmployeeQualificationDto,  TblEmployeeQualification>();
        }
    }
}
