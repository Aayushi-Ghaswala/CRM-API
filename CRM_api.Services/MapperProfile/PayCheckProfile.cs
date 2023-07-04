using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.MapperProfile
{
    public class PayCheckProfile : Profile
    {
        public PayCheckProfile()
        {
            CreateMap<AddPayCheckDto, TblPayCheck>().ReverseMap();
            CreateMap<TblPayCheck, PayCheckDto>();
            CreateMap<UpdatePayCheckDto, TblPayCheck>();

            CreateMap<TblPayCheck, PayCheckDto>().ReverseMap();
            CreateMap<Response<TblPayCheck>, ResponseDto<PayCheckDto>>();
        }
    }
}
