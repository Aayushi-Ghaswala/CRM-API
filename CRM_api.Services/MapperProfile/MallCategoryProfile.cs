using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.MapperProfile
{
    public class MallCategoryProfile : Profile
    {
        public MallCategoryProfile()
        {
            CreateMap<AddMallCategoryDto, TblWbcMallCategory>();
            CreateMap<UpdateMallCategoryDto, TblWbcMallCategory>();
            CreateMap<TblWbcMallCategory, MallCategoryDto>();
            CreateMap<Response<TblWbcMallCategory>, ResponseDto<MallCategoryDto>>();
        }
    }
}
