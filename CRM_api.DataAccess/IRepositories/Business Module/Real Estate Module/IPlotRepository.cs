using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Real_Estate_Module
{
    public interface IPlotRepository
    {
        Task<Response<TblPlotMaster>> GetPlots(int? projectId, string? purpose, string? search, SortingParams sortingParams, string? assignStatus);
        Task<int> AddPlot(TblPlotMaster plotMaster);
        Task<int> UpdatePlot(TblPlotMaster plotMaster);
        Task<int> DeletePlot(int id);
    }
}
