using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class WBCProfile : Profile
    {
        public WBCProfile()
        {
            CreateMap<TblWbcTypeMaster, WbcTypeDto>();
            CreateMap<WbcGPResponseModel, WbcGPResponseDto>();

            CreateMap<TblWbcSchemeMaster, WBCSchemeMasterDto>();
            CreateMap<Response<TblWbcSchemeMaster>, ResponseDto<WBCSchemeMasterDto>>();
            CreateMap<AddWBCSchemeDto, TblWbcSchemeMaster>();
            CreateMap<UpdateWBCSchemeDto, TblWbcSchemeMaster>();

            CreateMap<Response<WbcGPResponseModel>, ResponseDto<WbcGPResponseDto>>();
            CreateMap<Response<TblWbcTypeMaster>, ResponseDto<WbcTypeDto>>();

            CreateMap<TblGoldPointCategory, GoldPointCategoryDto>();
            CreateMap<UserNameResponse, UserNameDto>();
            CreateMap<Response<UserNameResponse>, ResponseDto<UserNameDto>>();
            CreateMap<WBCTypeResponse, WbcTypeDto>()
                .ForMember(dest => dest.WbcType, opt => opt.MapFrom(src => src.TypeName));
            CreateMap<Response<WBCTypeResponse>, ResponseDto<WbcTypeDto>>();
            CreateMap<TblGoldPoint, GoldPointDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.TblUserMaster.UserName))
                .ForMember(dest => dest.PointCategory, opt => opt.MapFrom(src => src.TblGoldPointCategory.PointCategory));
            CreateMap<Response<TblGoldPoint>, ResponseDto<GoldPointDto>>();
            CreateMap<LedgerResponse<TblGoldPoint>, GoldPointResponseDto<GoldPointDto>>();

            CreateMap<ReferenceTrackingResponseModel, ReferenceTrackingResponseDto>();
            CreateMap<Response<ReferenceTrackingResponseModel>, ResponseDto<ReferenceTrackingResponseDto>>();
        }
    }
}
