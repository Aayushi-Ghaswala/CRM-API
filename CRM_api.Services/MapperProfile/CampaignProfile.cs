using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            CreateMap<AddCampaignDto, TblCampaignMaster>().ReverseMap();
            CreateMap<TblCampaignMaster, CampaignDto>();
            CreateMap<UpdateCampaignDto, TblCampaignMaster>();

            CreateMap<TblCampaignMaster, CampaignDto>().ReverseMap();
            CreateMap<Response<TblCampaignMaster>, ResponseDto<CampaignDto>>();
        }
    }
}
