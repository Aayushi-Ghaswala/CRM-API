using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class WBCProfile : Profile
    {
        public WBCProfile()
        {
            CreateMap<TblWbcTypeMaster, WbcTypeDto>();
            CreateMap<TblSubInvesmentType, SubInvestmentTypeDto>();
            CreateMap<TblSubsubInvType, SubSubInvestmentTypeDto>();
            CreateMap<WbcGPResponseModel, WbcGPResponseDto>();

            CreateMap<TblWbcSchemeMaster, WBCSchemeMasterDto>();
            CreateMap<Response<TblWbcSchemeMaster>, ResponseDto<WBCSchemeMasterDto>>();
            CreateMap<AddWBCSchemeDto, TblWbcSchemeMaster>();
            CreateMap<UpdateWBCSchemeDto, TblWbcSchemeMaster>();

            CreateMap<Response<WbcGPResponseModel>, ResponseDto<WbcGPResponseDto>>();
            CreateMap<Response<TblWbcTypeMaster>, ResponseDto<WbcTypeDto>>();
            CreateMap<Response<TblSubInvesmentType>, ResponseDto<SubInvestmentTypeDto>>();
            CreateMap<Response<TblSubsubInvType>, ResponseDto<SubSubInvestmentTypeDto>>();
        }
    }
}
