using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.MapperProfile
{
    public class StocksProfile : Profile
    {
        public StocksProfile()
        {
            CreateMap<AddSharekhanStocksDto, TblStockData>();
        }
    }
}
