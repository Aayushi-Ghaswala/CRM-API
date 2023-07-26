using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class MeetingParticipantProfile : Profile
    {
        public MeetingParticipantProfile()
        {
            CreateMap<AddMeetingParticipantDto, TblMeetingParticipant>().ReverseMap();
            CreateMap<TblMeetingParticipant, MeetingParticipantDto>();
            CreateMap<UpdateMeetingParticipantDto, TblMeetingParticipant>();

            CreateMap<TblMeetingParticipant, MeetingParticipantDto>().ReverseMap();
            CreateMap<Response<TblMeetingParticipant>, ResponseDto<MeetingParticipantDto>>();
        }
    }
}