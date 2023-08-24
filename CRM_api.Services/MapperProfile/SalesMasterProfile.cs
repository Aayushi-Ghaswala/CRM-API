using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class SalesMasterProfile : Profile
    {
        public SalesMasterProfile()
        {
            //CreateMap<TblMeetingMaster, MeetingMasterDto>();
            CreateMap<TblConversationHistoryMaster, ConversationHistoryDto>();
            CreateMap<AddConversationHistoryDto, TblConversationHistoryMaster>()
                .AfterMap((dto, model) =>
                {
                    model.Date = DateTime.Now;
                    model.IsDeleted = false;
                });
            CreateMap<UpdateConversationHistoryDto, TblConversationHistoryMaster>();
            CreateMap<Response<TblConversationHistoryMaster>, ResponseDto<ConversationHistoryDto>>();
        }
    }
}
