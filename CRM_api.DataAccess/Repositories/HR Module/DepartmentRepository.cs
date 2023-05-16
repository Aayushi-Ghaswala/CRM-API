using CRM_api.DataAccess.Context;
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
        public async Task<Response<TblDepartmentMaster>> GetDepartments(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblDepartmentMasters.Where(x => x.Isdeleted == false).Count() / pageResult);

            var depts = await _context.TblDepartmentMasters.Where(x => x.Isdeleted == false).Skip((page - 1) * (int)pageResult)
                                                     .Take((int)pageResult).ToListAsync();

            var departmentResponse = new Response<TblDepartmentMaster>()
            {
                Values = depts,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return departmentResponse;
        }
        #endregion

        #region Get Department by Id
        public async Task<TblDepartmentMaster> GetDepartmentById(int id)
        {
            var dept = await _context.TblDepartmentMasters.FirstAsync(x => x.DepartmentId == id);
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
            var department = await _context.TblDepartmentMasters.FindAsync(departmentMaster.DepartmentId);

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
