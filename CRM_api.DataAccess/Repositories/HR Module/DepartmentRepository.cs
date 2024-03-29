﻿using CRM_api.DataAccess.Context;
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
        public async Task<Response<TblDepartmentMaster>> GetDepartments(string search, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<TblDepartmentMaster> filterData = new List<TblDepartmentMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblDepartmentMaster>(search).Where(x => x.Isdeleted != true).AsQueryable(); ;
            }
            else
            {
                filterData = _context.TblDepartmentMasters.Where(x => x.Isdeleted != true).AsQueryable();
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

        #region Add Department
        public async Task<int> AddDepartment(TblDepartmentMaster departmentMaster)
        {
            if (_context.TblDepartmentMasters.Any(x => x.Name == departmentMaster.Name && x.Isdeleted != true))
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
            
            department.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

    }
}
