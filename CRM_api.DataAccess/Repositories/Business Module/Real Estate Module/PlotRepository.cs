using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Real_Estate_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Real_Estate_Module
{
    public class PlotRepository : IPlotRepository
    {
        private readonly CRMDbContext _context;

        public PlotRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Plot
        public async Task<Response<TblPlotMaster>> GetPlots(int? projectId, string? purpose, string? search, SortingParams sortingParams, string? assignStatus)
        {
            double? pageCount = 0;
            IQueryable<TblPlotMaster> plots = new List<TblPlotMaster>().AsQueryable();

            if (search != null)
                plots = _context.Search<TblPlotMaster>(search).Where(x => (projectId == null || x.ProjectId == projectId) && (purpose == null || x.Purpose.ToLower().Equals(purpose.ToLower()))).Include(x => x.TblProjectMaster).AsQueryable();
            else
                plots = _context.TblPlotMasters.Where(x => (projectId == null || x.ProjectId == projectId) && (purpose == null || x.Purpose.ToLower().Equals(purpose.ToLower()))).Include(x => x.TblProjectMaster).AsQueryable();

            if (assignStatus == "Allocated")
                plots = plots.Where(x => (assignStatus == null || _context.TblMgainPlotData.Any(m => m.PlotId == x.Id)));
            else if (assignStatus == "UnAllocated")
                plots = plots.Where(x => (assignStatus == null || !_context.TblMgainPlotData.Any(m => m.PlotId == x.Id)));

            pageCount = Math.Ceiling(plots.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(plots, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var plotResponse = new Response<TblPlotMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return plotResponse;
        }
        #endregion

        #region Add Plot
        public async Task<int> AddPlot(TblPlotMaster plotMaster)
        {
            if (_context.TblPlotMasters.Any(x => x.ProjectId == plotMaster.ProjectId && x.PlotNo == plotMaster.PlotNo))
                return 0;

            await _context.TblPlotMasters.AddAsync(plotMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Plot
        public async Task<int> UpdatePlot(TblPlotMaster plotMaster)
        {
            if (_context.TblPlotMasters.Any(x => x.ProjectId == plotMaster.ProjectId && x.PlotNo == plotMaster.PlotNo && x.Id != plotMaster.Id))
                return 0;

            _context.TblPlotMasters.Update(plotMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Plot
        public async Task<int> DeletePlot(int id)
        {
            var plot = await _context.TblPlotMasters.FindAsync(id);

            if (plot == null)
                return 0;

            _context.TblPlotMasters.Remove(plot);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
