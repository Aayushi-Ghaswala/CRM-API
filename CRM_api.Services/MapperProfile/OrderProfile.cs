using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.MapperProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderStatusDto, TblOrderStatus>()
                .AfterMap((dto, order) =>
                {
                    order.IsDeleted = false;
                });
            CreateMap<UpdateOrderStatusDto, TblOrderStatus>();
            CreateMap<TblOrderStatus, OrderStatusDto>().ReverseMap();
            CreateMap<Response<TblOrderStatus>, ResponseDto<OrderStatusDto>>();
            CreateMap<TblOrderDetail, OrderDetailDto>().ReverseMap();
            CreateMap<TblOrder, OrderDto>();
            CreateMap<Response<TblOrder>, ResponseDto<OrderDto>>();
        }
    }
}
