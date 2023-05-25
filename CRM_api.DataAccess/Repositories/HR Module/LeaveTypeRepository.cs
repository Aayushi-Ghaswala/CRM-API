using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly CRMDbContext _context;

        public LeaveTypeRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get LeaveTypes
        public async Task<Response<TblLeaveType>> GetLeaveTypes(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblLeaveTypes.Where(x => x.Isdeleted != true).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblLeaveType>(search).Where(x => x.Isdeleted != true).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var leaveTypeResponse = new Response<TblLeaveType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return leaveTypeResponse;
        }
        #endregion

        #region Get LeaveType by Id
        public async Task<TblLeaveType> GetLeaveTypeById(int id)
        {
            var leaveType = await _context.TblLeaveTypes.FirstAsync(x => x.LeaveId == id && x.Isdeleted != true);
            return leaveType;
        }
        #endregion

        #region Get LeaveType by Name
        public async Task<TblLeaveType> GetLeaveTypeByName(string Name)
        {
            var leaveType = await _context.TblLeaveTypes.FirstAsync(x => x.Name.ToLower() == Name.ToLower() && x.Isdeleted != true);
            return leaveType;
        }
        #endregion

        #region Add LeaveType
        public async Task<int> AddLeaveType(TblLeaveType leaveType)
        {
            if (_context.TblLeaveTypes.Any(x => x.Name == leaveType.Name))
                return 0;

            _context.TblLeaveTypes.Add(leaveType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update LeaveType
        public async Task<int> UpdateLeaveType(TblLeaveType leaveType)
        {
            var leaveTypes = _context.TblLeaveTypes.AsNoTracking().Where(x => x.LeaveId == leaveType.LeaveId);

            if (leaveTypes == null) return 0;

            _context.TblLeaveTypes.Update(leaveType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate LeaveType
        public async Task<int> DeactivateLeaveType(int id)
        {
            var leaveType = await _context.TblLeaveTypes.FindAsync(id);

            if(leaveType == null) return 0;

            leaveType.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}