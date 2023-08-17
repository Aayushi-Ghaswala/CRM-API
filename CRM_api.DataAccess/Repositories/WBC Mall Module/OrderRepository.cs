using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace CRM_api.DataAccess.Repositories.WBC_Mall_Module
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CRMDbContext _context;

        public OrderRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Orders
        public async Task<Response<TblOrder>> GetOrders(int? statusId, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblOrder>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblOrder>(search).Where(x => (statusId == null || x.OrderStatusId == statusId)).Include(x => x.TblCityMaster)
                                                              .Include(x => x.TblStateMaster).Include(x => x.TblCountryMaster).Include(x => x.TblOrderStatus)
                                                              .Include(x => x.TblOrderDetails).ThenInclude(x => x.Product).Include(x => x.TblUserMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblOrders.Where(x => (statusId == null || x.OrderStatusId == statusId)).Include(x => x.TblCityMaster)
                                               .Include(x => x.TblStateMaster).Include(x => x.TblCountryMaster).Include(x => x.TblOrderStatus)
                                               .Include(x => x.TblOrderDetails).ThenInclude(x => x.Product).Include(x => x.TblUserMaster).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply PAgination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var orderResponse = new Response<TblOrder>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return orderResponse;
        }
        #endregion

        #region Get Order By Id
        public async Task<TblOrder> GetOrderById(int id)
        {
            var order = await _context.TblOrders.Where(x => x.OrderId == id).Include(x => x.TblCityMaster).Include(x => x.TblStateMaster).Include(x => x.TblCountryMaster).Include(x => x.TblOrderStatus)
                                                              .Include(x => x.TblOrderDetails).Include(x => x.TblUserMaster).AsNoTracking().FirstOrDefaultAsync();

            if (order is null) return null;

            return order;
        }
        #endregion

        #region Check Tracking Number Exist
        public async Task<int> CheckTrackingNoExist(int id, string trackingNo)
        {
            if (_context.TblOrders.Any(x => x.OrderId != id && x.TrackingNumber.Equals(trackingNo)))
                return 0;
            return 1;
        }
        #endregion

        #region Update Order
        public async Task<int> UpdateOrder(TblOrder order)
        {
            _context.TblOrders.Update(order);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
