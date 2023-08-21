using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Real_Estate_Module
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly CRMDbContext _context;

        public ProjectRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Project
        public async Task<Response<TblProjectMaster>> GetProjects(bool? isActive, string? search, SortingParams sortingParams)
        {
            double? pageCount = 0;
            IQueryable<TblProjectMaster> projects = new List<TblProjectMaster>().AsQueryable();

            if (search != null)
                projects = _context.Search<TblProjectMaster>(search).Where(x => (isActive == null || x.IsActive == isActive)).Include(x => x.TblProjectTypeDetail).AsQueryable();
            else
                projects = _context.TblProjectMasters.Where(x => (isActive == null || x.IsActive == isActive)).Include(x => x.TblProjectTypeDetail).AsQueryable();

            pageCount = Math.Ceiling(projects.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(projects, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var projectResponse = new Response<TblProjectMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return projectResponse;
        }
        #endregion

        #region Add Project
        public async Task<int> AddProject(TblProjectMaster projectMaster)
        {
            if (_context.TblProjectMasters.Any(x => x.Name == projectMaster.Name))
                return 0;

            await _context.TblProjectMasters.AddAsync(projectMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Project
        public async Task<int> UpdateProject(TblProjectMaster projectMaster)
        {
            if (_context.TblProjectMasters.Any(x => x.Name == projectMaster.Name && x.Id != projectMaster.Id))
                return 0;

            _context.TblProjectMasters.Update(projectMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Project
        public async Task<int> DeactivateProject(int id)
        {
            var project = await _context.TblProjectMasters.FindAsync(id);

            if (project == null || _context.TblPlotMasters.Any(x => x.ProjectId == id))
                return 0;

            project.IsActive = false;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
