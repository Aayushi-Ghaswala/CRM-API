using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly CRMDbContext _context;

        public DesignationRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Designation
        public async Task<Response<TblDesignationMaster>> GetDesignation(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblDesignationMasters.Where(x => x.Isdeleted != true).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblDesignationMaster>(search).Where(x => x.Isdeleted != true).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var departmentResponse = new Response<TblDesignationMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return departmentResponse;
        }
        #endregion

        #region Get Designation By
        public async Task<IEnumerable<TblDesignationMaster>> GetDesignationByDepartment(int departmentId)
        {
            var depts = await _context.TblDesignationMasters.Include(d => d.DepartmentMaster).Where(d => d.DepartmentId == departmentId && d.Isdeleted != true).ToListAsync();
            return depts;
        }

        public async Task<TblDesignationMaster> GetDesignationById(int id)
        {
            var dept = await _context.TblDesignationMasters.Include(d => d.DepartmentMaster).FirstAsync(x => x.DesignationId == id);
            return dept;
        }
        #endregion

        #region Add Designation
        public async Task<int> AddDesignation(TblDesignationMaster designationMaster)
        {
            if (_context.TblDesignationMasters.Any(x => x.Name == designationMaster.Name))
                return 0;

            _context.TblDesignationMasters.Add(designationMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Designation
        public async Task<int> UpdateDesignation(TblDesignationMaster designationMaster)
        {
            var designation = _context.TblDesignationMasters.AsNoTracking().Where(x => x.DesignationId == designationMaster.DesignationId);
            if (designation == null) return 0;

            _context.TblDesignationMasters.Update(designationMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Designation
        public async Task<int> DeactivateDesignation(int id)
        {
            var designation = await _context.TblDesignationMasters.FindAsync(id);
            if (designation == null) return 0;

            designation.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
