using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.WBC_Mall_Module
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly CRMDbContext _context;

        public OrderStatusRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Order Statuses
        public async Task<Response<TblOrderStatus>> GetOrderStatuses(string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblOrderStatus>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblOrderStatus>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblOrderStatuses.Where(x => x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var orderStatusResponse = new Response<TblOrderStatus>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return orderStatusResponse;
        }
        #endregion

        #region Add Order Status
        public async Task<int> AddOrderStatus(TblOrderStatus tblOrderStatus)
        {
            if (_context.TblOrderStatuses.Any(x => x.Statusname.ToLower().Equals(tblOrderStatus.Statusname.ToLower()) && x.IsDeleted != true))
                return 0;

            _context.TblOrderStatuses.Add(tblOrderStatus);
            return await _context.SaveChangesAsync();
        }
        #endregion


        #region Update Order Status
        public async Task<int> UpdateOrderStatus(TblOrderStatus tblOrderStatus)
        {
            if (_context.TblOrderStatuses.Any(x => x.Id != tblOrderStatus.Id && x.Statusname.ToLower().Equals(tblOrderStatus.Statusname.ToLower()) && x.IsDeleted != true))
                return 0;

            _context.TblOrderStatuses.Update(tblOrderStatus);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region De-Activate Order Status
        public async Task<int> DeActivateOrderStatus(int id)
        {
            var orderStatus = await _context.TblOrderStatuses.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
            if (orderStatus is null) return 0;

            orderStatus.IsDeleted = true;
            _context.TblOrderStatuses.Update(orderStatus);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
