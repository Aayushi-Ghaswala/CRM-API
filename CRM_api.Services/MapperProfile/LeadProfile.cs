using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
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

            CreateMap<TblLeadMaster, LeadDto>().ReverseMap();
            CreateMap<Response<TblLeadMaster>, ResponseDto<LeadDto>>();

            CreateMap<TblInvesmentType, InvestmentTypeDto>().ReverseMap();
            CreateMap<Response<TblInvesmentType>, ResponseDto<InvestmentTypeDto>>();
        }
    }
}
