using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class InvestmentProfile : Profile
    {
        public InvestmentProfile()
        {
            CreateMap<TblInvesmentType, InvestmentTypeDto>();
            CreateMap<TblSubInvesmentType, SubInvestmentTypeDto>()
                .ForMember(dest => dest.SubInvestmentType, opt => opt.MapFrom(src => src.InvestmentType));
            CreateMap<TblSubsubInvType, SubsubInvTypeDto>()
                .ForMember(dest => dest.SubsubInvType, opt => opt.MapFrom(src => src.SubInvType));

            CreateMap<Response<TblInvesmentType>, ResponseDto<InvestmentTypeDto>>();
            CreateMap<Response<TblSubInvesmentType>, ResponseDto<SubInvestmentTypeDto>>();
            CreateMap<Response<TblSubsubInvType>, ResponseDto<SubsubInvTypeDto>>();

            CreateMap<AddInvestmentTypeDto, TblInvesmentType>();
            CreateMap<AddSubInvestmentTypeDto, TblSubInvesmentType>();
            CreateMap<AddSubsubInvTypeDto, TblSubsubInvType>();

            CreateMap<UpdateInvestmentTypeDto, TblInvesmentType>();
            CreateMap<UpdateSubInvestmentTypeDto, TblSubInvesmentType>();
            CreateMap<UpdateSubsubInvTypeDto, TblSubsubInvType>();
        }
    }
}
