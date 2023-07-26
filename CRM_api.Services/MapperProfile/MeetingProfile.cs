using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class MeetingProfile : Profile
    {
        public MeetingProfile()
        {
            CreateMap<AddMeetingDto, TblMeetingMaster>().ReverseMap();
            CreateMap<TblMeetingMaster, MeetingDto>();
            CreateMap<UpdateMeetingDto, TblMeetingMaster>();

            CreateMap<TblMeetingMaster, MeetingDto>().ReverseMap();
            CreateMap<Response<TblMeetingMaster>, ResponseDto<MeetingDto>>();
        }
    }
}