using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.MapperProfile
{
    public class MallProductProfile : Profile
    {
        public MallProductProfile()
        {
            CreateMap<AddMallProductDto, TblWbcMallProduct>()
                .AfterMap((dto, product) =>
                {
                    product.ProdDateAdded = DateTime.Now;
                });
            CreateMap<UpdateMallProductDto, TblWbcMallProduct>();
            CreateMap<TblWbcMallProduct, MallProductDto>();
            CreateMap<TblProductImg, ProductImageDto>().ReverseMap();
            CreateMap<Response<TblWbcMallProduct>, ResponseDto<MallProductDto>>();
        }
    }
}
