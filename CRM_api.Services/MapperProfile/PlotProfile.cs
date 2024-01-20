using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.RealEstateModule;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.AddDataDto.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class PlotProfile : Profile
    {
        public PlotProfile()
        {
            CreateMap<TblPlotMaster, PlotMasterDto>();
            CreateMap<Response<TblPlotMaster>, ResponseDto<PlotMasterDto>>();
            CreateMap<AddPlotDto, TblPlotMaster>();
            CreateMap<UpdatePlotDto, TblPlotMaster>();
            CreateMap<PlotResponse<TblPlotMaster>, PlotResponseDto<PlotMasterDto>>();
        }
    }
}
