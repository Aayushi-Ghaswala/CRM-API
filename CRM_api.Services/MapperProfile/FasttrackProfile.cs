using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.Fasttrack_Module;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class FasttrackProfile : Profile
    {
        public FasttrackProfile()
        {
            CreateMap<UpdateFasttrackSchemeDto, TblFasttrackSchemeMaster>();
            CreateMap<UpdateFasttrackLevelCommissionDto, TblFasttrackLevelCommission>();
            
            CreateMap<FasttrackResponseModel, FasttrackResponseDto>()
                             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserMaster.UserName));
            CreateMap<Response<FasttrackResponseModel>, ResponseDto<FasttrackResponseDto>>();
            
            CreateMap<FasttrackInvTypeResponse, FasttrackInvTypeResponseDto>();
            CreateMap<Response<FasttrackInvTypeResponse>, ResponseDto<FasttrackInvTypeResponseDto>>();
            CreateMap<UserNameResponse, UserNameDto>();
            CreateMap<Response<UserNameResponse>, ResponseDto<UserNameDto>>();
            CreateMap<TblFasttrackLedger, FasttrackLedgerDto>();
            CreateMap<Response<TblFasttrackLedger>, ResponseDto<FasttrackLedgerDto>>();
            CreateMap<LedgerResponse<TblFasttrackLedger>, GoldPointResponseDto<FasttrackLedgerDto>>();
            CreateMap<AddFasttrackBenefitsDto, TblFasttrackBenefits>();
            CreateMap<UpdateFasttrackBenefitsDto, TblFasttrackBenefits>();
            CreateMap<TblFasttrackBenefits, FasttrackBenefitsResponseDto>();
            CreateMap<TblFasttrackSchemeMaster, FasttrackSchemeResponseDto>();
            CreateMap<TblFasttrackLevelCommission, FasttrackLevelCommissionResponseDto>();
        }
    }
}
