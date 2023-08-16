using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.MapperProfile
{
    public class LeadProfile : Profile
    {
        public LeadProfile()
        {
            CreateMap<AddLeadDto, TblLeadMaster>().ReverseMap();
            CreateMap<TblLeadMaster, LeadDto>();
            CreateMap<UpdateLeadDto, TblLeadMaster>();
            CreateMap<TblLeadMaster, LeadCSVDto>()
                .ForMember(lead => lead.AssignUser, opt => opt.MapFrom(src => src.AssignUser.Name))
                .ForMember(lead => lead.ReferredUser, opt => opt.MapFrom(src => src.ReferredUser.UserName))
                .ForMember(lead => lead.Campaign, opt => opt.MapFrom(src => src.CampaignMaster.Name))
                .ForMember(lead => lead.City, opt => opt.MapFrom(src => src.CityMaster.CityName))
                .ForMember(lead => lead.State, opt => opt.MapFrom(src => src.StateMaster.StateName))
                .ForMember(lead => lead.Country, opt => opt.MapFrom(src => src.CountryMaster.CountryName))
                .ForMember(lead => lead.Status, opt => opt.MapFrom(src => src.StatusMaster.Name));

            CreateMap<TblLeadMaster, LeadDto>().ReverseMap();

            CreateMap<TblInvesmentType, InvestmentTypeDto>().ReverseMap();
            CreateMap<Response<TblInvesmentType>, ResponseDto<InvestmentTypeDto>>();
            CreateMap<Response<TblLeadMaster>, ResponseDto<LeadDto>>();

            CreateMap<TblInvesmentType, InvestmentTypeDto>().ReverseMap();
            CreateMap<Response<TblInvesmentType>, ResponseDto<InvestmentTypeDto>>();
        }
    }
}
