using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CRMDbContext _context;

        public EmployeeRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get all employees
        public async Task<Response<TblEmployeeMaster>> GetEmployees(string search, SortingParams sortingParams)
        {
            double pageCount = 0;
            IQueryable<TblEmployeeMaster> filterData = new List<TblEmployeeMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblEmployeeMaster>(search).Where(x => x.IsActive != false).Include(x => x.TblDepartmentMaster).Include(x => x.TblDesignationMaster).Include(x => x.TblCityMaster).Include(x => x.TblStateMaster).Include(x => x.TblCountryMaster).Include(x => x.TblEmployeeExperiences).Include(x => x.TblEmployeeQualifications).AsQueryable();
            }
            else
            {
                filterData = _context.TblEmployeeMasters.Where(x => x.IsActive != false).Include(x => x.TblDepartmentMaster).Include(x => x.TblDesignationMaster).Include(x => x.TblCityMaster).Include(x => x.TblStateMaster).Include(x => x.TblCountryMaster).Include(x => x.TblEmployeeExperiences).Include(x => x.TblEmployeeQualifications).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var employeesResponse = new Response<TblEmployeeMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return employeesResponse;
        }
        #endregion

        #region Add Employee
        public async Task<TblEmployeeMaster> AddEmployee(TblEmployeeMaster employeeMaster)
        {
            if (_context.TblEmployeeMasters.Any(x => x.Name == employeeMaster.Name && x.IsActive != true))
                return null;

            _context.TblEmployeeMasters.Add(employeeMaster);
            await _context.SaveChangesAsync();
            return employeeMaster;
        }
        #endregion

        #region Add Employee Qualification
        public async Task<int> AddEmployeeQualification(TblEmployeeQualification employeeQualification)
        {
            if (_context.TblEmployeeQualifications.Any(x => x.EmpId == employeeQualification.EmpId && x.Degree == employeeQualification.Degree))
                return 0;

            await _context.TblEmployeeQualifications.AddAsync(employeeQualification);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Employee Experience
        public async Task<int> AddEmployeeExperience(TblEmployeeExperience employeeExperience)
        {
            if (_context.TblEmployeeExperiences.Any(x => x.EmpId == employeeExperience.EmpId && x.CompanyName == employeeExperience.CompanyName))
                return 0;

            await _context.TblEmployeeExperiences.AddAsync(employeeExperience);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Employee
        public async Task<int> UpdateEmployee(TblEmployeeMaster employeeMaster)
        {
            var employee = _context.TblEmployeeMasters.AsNoTracking().Where(x => x.Id == employeeMaster.Id || (x.Name == employeeMaster.Name && x.Id != employeeMaster.Id));
            if (employee == null) return 0;

            _context.TblEmployeeMasters.Update(employeeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Employee Qualification
        public async Task<int> UpdateEmployeeQualification(TblEmployeeQualification employeeQualification)
        {
            if (_context.TblEmployeeQualifications.Any(x => x.Id != employeeQualification.Id && x.EmpId == employeeQualification.EmpId && x.Degree == employeeQualification.Degree))
                return 0;

            _context.TblEmployeeQualifications.Update(employeeQualification);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Employee Experience 
        public async Task<int> UpdateEmployeeExperience(TblEmployeeExperience employeeExperience)
        {
            if (_context.TblEmployeeExperiences.Any(x => x.Id != employeeExperience.Id && x.EmpId == employeeExperience.EmpId && x.CompanyName == employeeExperience.CompanyName))
                return 0;

            _context.TblEmployeeExperiences.Update(employeeExperience);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Employee
        public async Task<int> DeactivateEmployee(int id)
        {
            var employee = await _context.TblEmployeeMasters.FindAsync(id);

            if (employee == null) return 0;

            var employeeExperience = await _context.TblEmployeeExperiences.Where(x => x.EmpId == id).ToListAsync();
            var employeeQualification = await _context.TblEmployeeQualifications.Where(x => x.EmpId == id).ToListAsync();
            employee.IsActive = false;

            _context.TblEmployeeExperiences.RemoveRange(employeeExperience);
            _context.TblEmployeeQualifications.RemoveRange(employeeQualification);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Employee Qualification
        public async Task<int> DeleteEmployeeQualification(int id)
        {
            var employeeQualification = await _context.TblEmployeeQualifications.FindAsync(id);

            if (employeeQualification == null) return 0;

            _context.TblEmployeeQualifications.Remove(employeeQualification);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Employee Experience
        public async Task<int> DeleteEmployeeExperience(int id)
        {
            var employeeQualification = await _context.TblEmployeeExperiences.FindAsync(id);

            if (employeeQualification == null) return 0;

            _context.TblEmployeeExperiences.Remove(employeeQualification);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
