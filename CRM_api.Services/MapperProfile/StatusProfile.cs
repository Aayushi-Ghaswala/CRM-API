using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class StatusProfile : Profile
    {
        public StatusProfile()
        {
            CreateMap<AddStatusDto, TblStatusMaster>().ReverseMap();
            CreateMap<TblStatusMaster, StatusDto>();
            CreateMap<UpdateStatusDto, TblStatusMaster>();

            CreateMap<TblStatusMaster, StatusDto>().ReverseMap();
            CreateMap<Response<TblStatusMaster>, ResponseDto<StatusDto>>();
        }
    }
}
