using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.DataAccess.ResponseModel.Stocks_Module;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class StocksProfile : Profile
    {
        public StocksProfile()
        {
            CreateMap<AddSherkhanStocksDto, TblStockData>();
            CreateMap<AddSherkhanAllClientStockDto, TblStockData>();
            CreateMap<AddFNONSETradeListDto, TblStockData>();
            CreateMap<AddJainamStocksDto, TblStockData>()
                .ForMember(dest => dest.StScripname, opt => opt.MapFrom(src => src.ScriptName))
                .ForMember(dest => dest.StSettno, opt => opt.MapFrom(src => src.Narration))
                .ForMember(dest => dest.StDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.StNetsharerate, opt => opt.MapFrom(src => src.NetRate))
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

            CreateMap<TblStockData, StockMasterDto>().ReverseMap();
            CreateMap<Response<TblStockData>, ResponseDto<StockMasterDto>>();
            CreateMap<StocksResponse<TblStockData>, StockResponseDto<StockMasterDto>>();
            CreateMap<TblStockData, ScriptNamesDto>();
            CreateMap<Response<TblStockData>, ResponseDto<ScriptNamesDto>>();
            CreateMap<TblStockData, UserNameDto>();
            CreateMap<Response<TblStockData>, ResponseDto<UserNameDto>>();
            CreateMap<ScriptNameResponse, ScriptNamesDto>();
            CreateMap<Response<ScriptNameResponse>, ResponseDto<ScriptNamesDto>>();
            CreateMap<AddScripDto, TblScripMaster>();
            CreateMap<StocksDashboardIntraDeliveryResponse, StocksDashboardIntraDeliveryDto>();
            CreateMap<TblScripMaster, ScripMasterDto>();
            CreateMap<Response<TblScripMaster>, ResponseDto<ScripMasterDto>>();
        }
    }
}
