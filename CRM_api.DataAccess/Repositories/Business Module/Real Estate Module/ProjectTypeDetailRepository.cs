using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Real_Estate_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Real_Estate_Module
{
    public class ProjectTypeDetailRepository : IProjectTypeDetailRepository
    {
        private readonly CRMDbContext _context;

        public ProjectTypeDetailRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Project Type Details
        public async Task<Response<TblProjectTypeDetail>> GetProjectTypeDetails(int? projectTypeId, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterdata = new List<TblProjectTypeDetail>().AsQueryable();

            if (search is not null)
            {
                filterdata = _context.Search<TblProjectTypeDetail>(search).Where(x => projectTypeId == null || x.ProjectTypeId == projectTypeId).Include(x => x.TblProjectTypeMaster).AsQueryable();
            }
            else
            {
                filterdata = _context.TblProjectTypeDetails.Where(x => projectTypeId == null || x.ProjectTypeId == projectTypeId).Include(x => x.TblProjectTypeMaster).AsQueryable();
            }

            pageCount = Math.Ceiling(filterdata.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterdata, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination 
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var projectDetailsResponse = new Response<TblProjectTypeDetail>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return projectDetailsResponse;
        }
        #endregion

        #region Get Project Types
        public async Task<Response<TblProjectTypeMaster>> GetProjectTypes(string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterdata = new List<TblProjectTypeMaster>().AsQueryable();

            if (search is not null)
            {
                filterdata = _context.Search<TblProjectTypeMaster>(search).AsQueryable();
            }
            else
            {
                filterdata = _context.TblProjectTypeMasters.AsQueryable();
            }

            pageCount = Math.Ceiling(filterdata.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterdata, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination 
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var projectTypesResponse = new Response<TblProjectTypeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return projectTypesResponse;
        }
        #endregion

        #region Add Project Type Detail
        public async Task<int> AddProjectTypeDetail(TblProjectTypeDetail tblProjectTypeDetail)
        {
            if (_context.TblProjectTypeDetails.Any(x => x.ProjectTypeDetail.ToLower().Equals(tblProjectTypeDetail.ProjectTypeDetail.ToLower())))
                return 0;

            _context.TblProjectTypeDetails.Add(tblProjectTypeDetail);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Project Type Detail
        public async Task<int> UpdateProjectTypeDetail(TblProjectTypeDetail tblProjectTypeDetail)
        {
            if (_context.TblProjectTypeDetails.Any(x => x.Id != tblProjectTypeDetail.Id && x.ProjectTypeDetail.ToLower().Equals(tblProjectTypeDetail.ProjectTypeDetail.ToLower())))
                return 0;

            _context.TblProjectTypeDetails.Update(tblProjectTypeDetail);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Project Type Detail
        public async Task<int> DeleteProjectTypeDetail(int id)
        {
            var projectTypeDetail = await _context.TblProjectTypeDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (projectTypeDetail is null || _context.TblProjectMasters.Any(x => x.ProjectTypeId == id))
                return 0;

            _context.TblProjectTypeDetails.Remove(projectTypeDetail);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
