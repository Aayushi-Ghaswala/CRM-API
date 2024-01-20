using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Real_Estate_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.AddDataDto.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Real_Estate_Module;

namespace CRM_api.Services.Services.Business_Module.Real_Estate_Module
{
    public class PlotService : IPlotService
    {
        private readonly IPlotRepository _plotRepository;
        private readonly IMapper _mapper;

        public PlotService(IPlotRepository plotRepository, IMapper mapper)
        {
            _plotRepository = plotRepository;
            _mapper = mapper;
        }

        #region Get All Plot
        public async Task<PlotResponseDto<PlotMasterDto>> GetPlotAsync(int? projectId, string? purpose, string? search, SortingParams sortingParams, string? assignStatus)
        {
            try
            {

                var plots = await _plotRepository.GetPlots(projectId, purpose, search, sortingParams, assignStatus);
                return _mapper.Map<PlotResponseDto<PlotMasterDto>>(plots);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Add Plot
        public async Task<int> AddPlotAsync(AddPlotDto addPlot)
        {
            var mapPlot = _mapper.Map<TblPlotMaster>(addPlot);
            mapPlot.Available_PlotValue = mapPlot.PlotValue;
            mapPlot.Available_SqFt = mapPlot.SqFt;
            return await _plotRepository.AddPlot(mapPlot);
        }
        #endregion

        #region Update Plot
        public async Task<int> UpdatePlotAsync(UpdatePlotDto updatePlot)
        {
            var mapPlot = _mapper.Map<TblPlotMaster>(updatePlot);
            return await _plotRepository.UpdatePlot(mapPlot);
        }
        #endregion

        #region Delete Plot
        public async Task<int> DeletePlotAsync(int id)
        {
            return await _plotRepository.DeletePlot(id);
        }
        #endregion
    }
}
