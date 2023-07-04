using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class UserLeaveRepository : IUserLeaveRepository
    {
        private readonly CRMDbContext _context;

        public UserLeaveRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get UserLeaves
        public async Task<Response<TblUserLeave>> GetUserLeaves(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserLeaves.Where(x => x.IsDeleted != true).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserLeave>(search).Where(x => x.IsDeleted != true).AsQueryable(); ;
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var userLeaveResponse = new Response<TblUserLeave>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return userLeaveResponse;
        }
        #endregion

        #region Get Leave by User
        public async Task<TblUserLeave> GetLeavesByUser(int userId)
        {
            var userLeave = await _context.TblUserLeaves.FirstAsync(x => x.RequestedBy == userId && x.IsDeleted != true);
            return userLeave;
        }
        #endregion

        #region Get UserLeave by Id
        public async Task<TblUserLeave> GetUserLeaveById(int id)
        {
            var userLeave = await _context.TblUserLeaves.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return userLeave;
        }
        #endregion

        #region Add UserLeave
        public async Task<int> AddUserLeave(TblUserLeave userLeaveMaster)
        {
            if (_context.TblUserLeaves.Any(x => x.FromDate == userLeaveMaster.FromDate))
                return 0;

            _context.TblUserLeaves.Add(userLeaveMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update UserLeave
        public async Task<int> UpdateUserLeave(TblUserLeave userLeaveMaster)
        {
            var userLeave = _context.TblUserLeaves.AsNoTracking().Where(x => x.Id == userLeaveMaster.Id);

            if (userLeave == null) return 0;

            _context.TblUserLeaves.Update(userLeaveMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate UserLeave
        public async Task<int> DeactivateUserLeave(int id)
        {
            var userLeave = await _context.TblUserLeaves.FindAsync(id);

            if (userLeave == null) return 0;

            userLeave.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

    }
}
