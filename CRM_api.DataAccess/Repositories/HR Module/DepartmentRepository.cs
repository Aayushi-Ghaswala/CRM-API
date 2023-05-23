using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CRMDbContext _context;

        public DepartmentRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Departments
        public async Task<Response<TblDepartmentMaster>> GetDepartments(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblDepartmentMasters.Where(x => x.IsDeleted != true).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblDepartmentMaster>(searchingParams);
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var departmentResponse = new Response<TblDepartmentMaster>()
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

        #region Get Department by Id
        public async Task<TblDepartmentMaster> GetDepartmentById(int id)
        {
            var dept = await _context.TblDepartmentMasters.FirstAsync(x => x.DepartmentId == id && x.Isdeleted != true);
            return dept;
        }
        #endregion

        #region Add Department
        public async Task<int> AddDepartment(TblDepartmentMaster departmentMaster)
        {
            if (_context.TblDepartmentMasters.Any(x => x.Name == departmentMaster.Name))
                return 0;

            _context.TblDepartmentMasters.Add(departmentMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Department
        public async Task<int> UpdateDepartment(TblDepartmentMaster departmentMaster)
        {
            var department = _context.TblDepartmentMasters.AsNoTracking().Where(x => x.DepartmentId == departmentMaster.DepartmentId);

            if (department == null) return 0;

            _context.TblDepartmentMasters.Update(departmentMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Department
        public async Task<int> DeactivateDepartment(int id)
        {
            var department = await _context.TblDepartmentMasters.FindAsync(id);

            if (department == null) return 0;

            var designation = _context.TblDesignationMasters.Where(x => x.DepartmentId == id);
            foreach (var item in designation)
            {
                item.Isdeleted = true;
            }
            department.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

    }
}
