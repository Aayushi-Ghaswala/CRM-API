using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.HR_Module;
using CRM_api.DataAccess.ResponseModel.User_Module;
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

        #region Add Department
        public async Task<int> AddDepartment(TblDepartmentMaster departmentMaster)
        {
            if (_context.TblDepartmentMasters.Any(x => x.Name == departmentMaster.Name))
                return 0;

            _context.TblDepartmentMasters.Add(departmentMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Department by Id
        public async Task<TblDepartmentMaster> GetDepartmentById(int id)
        {
            var dept = await _context.TblDepartmentMasters.FirstAsync(x => x.DepartmentId == id);
            return dept;
        }
        #endregion

        #region Get Departments
        public async Task<DepartmentResponse> GetDepartments(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblDepartmentMasters.Where(x => x.Isdeleted == false).Count() / pageResult);

            var depts = await _context.TblDepartmentMasters.Where(x => x.Isdeleted == false).Skip((page - 1) * (int)pageResult)
                                                     .Take((int)pageResult).ToListAsync();

            var departmentResponse = new DepartmentResponse()
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

        #region Update Department
        public async Task<int> UpdateDepartment(TblDepartmentMaster departmentMaster)
        {
            _context.TblDepartmentMasters.Update(departmentMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
