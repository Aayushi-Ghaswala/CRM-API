using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;

namespace CRM_api.Services.MapperProfile
{
    public class StocksProfile : Profile
    {
        public StocksProfile()
        {
            CreateMap<AddSharekhanStocksDto, TblStockData>();
            CreateMap<AddJainamStocksDto, TblStockData>()
                .ForMember(dest => dest.StScripname, opt => opt.MapFrom(src => src.ScriptName))
                .ForMember(dest => dest.StSettno, opt => opt.MapFrom(src => src.Narration))
                .ForMember(dest => dest.StDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.StRate, opt => opt.MapFrom(src => src.NetRate))
                .ForMember(dest => dest.StTransactionDetails, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.StNetcostvalue, opt => opt.MapFrom(src => src.NetAmount))
                .ForMember(dest => dest.FirmName, opt => opt.MapFrom(src => src.FirmName))
                .AfterMap((dto, data) =>
                {
                    var type = dto.BuyQty > 0 ? "B" : "S";
                    data.StType = type;

                    if (type.Equals("B"))
                        data.StQty = dto.BuyQty;
                    else
                        data.StQty = dto.SellQty;
                });
        }
    }
}
