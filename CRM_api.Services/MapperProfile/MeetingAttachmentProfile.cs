using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class MeetingAttachmentProfile : Profile
    {
        public MeetingAttachmentProfile()
        {
            CreateMap<AddMeetingAttachmentDto, TblMeetingAttachment>().ReverseMap();
            CreateMap<TblMeetingAttachment, MeetingAttachmentDto>();
            CreateMap<UpdateMeetingAttachmentDto, TblMeetingAttachment>();

            CreateMap<TblMeetingAttachment, MeetingAttachmentDto>().ReverseMap();
            CreateMap<Response<TblMeetingAttachment>, ResponseDto<MeetingAttachmentDto>>();
        }
    }
}