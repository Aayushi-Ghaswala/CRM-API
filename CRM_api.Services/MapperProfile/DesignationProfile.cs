using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.MapperProfile
{
    public class DesignationProfile : Profile
    {
        public DesignationProfile()
        {
            CreateMap<AddDesignationDto, TblDesignationMaster>();
            CreateMap<UpdateDesignationDto, TblDesignationMaster>();

            CreateMap<TblDesignationMaster, DesignationDto>().ReverseMap();
            CreateMap<Response<TblDesignationMaster>, ResponseDto<DesignationDto>>();
        }
    }
}
