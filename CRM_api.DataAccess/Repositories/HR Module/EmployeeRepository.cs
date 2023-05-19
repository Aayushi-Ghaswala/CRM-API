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
        public async Task<Response<TblUserMaster>> GetEmployees(int categoryId, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserMasters.Where(x => x.CatId == categoryId).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblUserMaster>(searchingParams);
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var employeesResponse = new Response<TblUserMaster>()
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

        #region Get Employee by Id
        public async Task<TblUserMaster> GetEmployeebyId(int id)
        {
            var user = await _context.TblUserMasters.Include(x => x.TblUserCategoryMaster).Include(x => x.TblUserCategoryMaster)
                                                    .Include(c => c.TblCountryMaster).Include(s => s.TblStateMaster)
                                                    .Include(ct => ct.TblCityMaster).FirstAsync(x => x.UserId == id);
            return user;
        }
        #endregion

        #region Add Employee
        public async Task<int> AddEmployee(TblUserMaster userMaster)
        {
            if (_context.TblUserMasters.Any(x => x.UserUname == userMaster.UserUname))
                return 0;

            _context.TblUserMasters.Add(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Employee
        public async Task<int> UpdateEmployee(TblUserMaster userMaster)
        {
            var employee = await _context.TblUserMasters.FindAsync(userMaster.UserId);

            if (employee == null) return 0;

            _context.TblUserMasters.Update(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Employee
        public async Task<int> DeactivateEmployee(int id)
        {
            var employee = await _context.TblUserMasters.FindAsync(id);

            if (employee == null) return 0;

            employee.UserIsactive = false;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
