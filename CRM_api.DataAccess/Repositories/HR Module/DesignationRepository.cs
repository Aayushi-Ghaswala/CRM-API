using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.HR_Module;
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

        #region Add Designation
        public async Task<int> AddDesignation(TblDesignationMaster designationMaster)
        {
            if (_context.TblDesignationMasters.Any(x => x.Name == designationMaster.Name))
                return 0;

            _context.TblDesignationMasters.Add(designationMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Designation
        public async Task<DesignationResponse> GetDesignation(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblDesignationMasters.Where(x => x.Isdeleted == false).Count() / pageResult);

            var designations = await _context.TblDesignationMasters.Include(d => d.DepartmentMaster).Where(x => x.Isdeleted == false).Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var departmentResponse = new DesignationResponse()
            {
                Values = designations,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return departmentResponse;
        }
        #endregion

        #region Get Designation By
        public async Task<IEnumerable<TblDesignationMaster>> GetDesignationByDepartment(int deptId)
        {
            var depts = await _context.TblDesignationMasters.Include(d => d.DepartmentMaster).Where(d => d.DepartmentId == deptId).ToListAsync();
            return depts;
        }

        public async Task<TblDesignationMaster> GetDesignationById(int id)
        {
            var dept = await _context.TblDesignationMasters.Include(d => d.DepartmentMaster).FirstAsync(x => x.DesignationId == id);
            return dept;
        }

        public async Task<int> UpdateDesignation(TblDesignationMaster designationMaster)
        {
            _context.TblDesignationMasters.Update(designationMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
