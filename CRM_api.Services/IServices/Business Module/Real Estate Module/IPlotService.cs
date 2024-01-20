using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.RealEstateModule;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.AddDataDto.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Real_Estate_Module
{
    public interface IPlotService
    {
        Task<PlotResponseDto<PlotMasterDto>> GetPlotAsync(int? projectId, string? purpose, string? search, SortingParams sortingParams, string? assignStatus);
        Task<int> AddPlotAsync(AddPlotDto addPlot);
        Task<int> UpdatePlotAsync(UpdatePlotDto updatePlot);
        Task<int> DeletePlotAsync(int id);
    }
}
