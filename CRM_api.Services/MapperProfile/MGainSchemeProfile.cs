using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class MGainSchemeProfile : Profile
    {
        public MGainSchemeProfile()
        {
            CreateMap<AddMGainSchemeDto, TblMgainSchemeMaster>();
            CreateMap<UpdateMGainSchemeDto, TblMgainSchemeMaster>();
            CreateMap<TblMgainSchemeMaster, MGainSchemeDto>();
            CreateMap<Response<TblMgainSchemeMaster>, ResponseDto<MGainSchemeDto>>();
        }
    }
}
