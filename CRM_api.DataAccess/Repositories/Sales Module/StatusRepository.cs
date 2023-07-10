using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Sales_Module
{
    public class StatusRepository : IStatusRepository
    {
        private readonly CRMDbContext _context;

        public StatusRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Statues
        public async Task<Response<TblStatusMaster>> GetStatues(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblStatusMasters.Where(x => x.IsDeleted != true).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblStatusMaster>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var statusResponse = new Response<TblStatusMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return statusResponse;
        }
        #endregion

        #region Get Status by Id
        public async Task<TblStatusMaster> GetStatusById(int id)
        {
            var status = await _context.TblStatusMasters.FirstAsync(x => x.Id == id && x.IsDeleted != true);
            return status;
        }
        #endregion

        #region Get Status by Name
        public async Task<TblStatusMaster> GetStatusByName(string Name)
        {
            var status = await _context.TblStatusMasters.FirstAsync(x => x.Name.ToLower().Contains(Name.ToLower()) && x.IsDeleted != true);
            return status;
        }
        #endregion

        #region Add Status
        public async Task<int> AddStatus(TblStatusMaster status)
        {
            if (_context.TblStatusMasters.Any(x => x.Name == status.Name))
                return 0;

            _context.TblStatusMasters.Add(status);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Status
        public async Task<int> UpdateStatus(TblStatusMaster status)
        {
            var statuss = _context.TblStatusMasters.AsNoTracking().Where(x => x.Id == status.Id);

            if (statuss == null) return 0;

            _context.TblStatusMasters.Update(status);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Status
        public async Task<int> DeactivateStatus(int id)
        {
            var status = await _context.TblStatusMasters.FindAsync(id);

            if(status == null) return 0;

            status.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}