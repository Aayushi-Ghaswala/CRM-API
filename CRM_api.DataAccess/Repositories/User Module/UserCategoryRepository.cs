using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.User_Module
{
    public class UserCategoryRepository : IUserCategoryRepository
    {
        private readonly CRMDbContext _context;

        public UserCategoryRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All User Category
        public async Task<Response<TblUserCategoryMaster>> GetUserCategories(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblUserCategoryMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserCategoryMaster>(search).Where(x => x.CatIsactive != false).AsQueryable();
            }
            else
            {
                filterData = _context.TblUserCategoryMasters.Where(x => x.CatIsactive != false).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var categoriesResponse = new Response<TblUserCategoryMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return categoriesResponse;
        }
        #endregion

        #region Add User Category
        public async Task<int> AddUserCategory(TblUserCategoryMaster tblUserCategory)
        {
            if (_context.TblUserCategoryMasters.Any(x => x.CatName.ToLower().Equals(tblUserCategory.CatName) && x.CatIsactive == true))
                return 0;

            _context.TblUserCategoryMasters.Add(tblUserCategory);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Category
        public async Task<int> UpdateUserCategory(TblUserCategoryMaster tblUserCategory)
        {
            var user = await _context.TblUserCategoryMasters.Where(x => x.CatId == tblUserCategory.CatId && x.CatIsactive == true).AsNoTracking().FirstOrDefaultAsync();
            if (user == null) return 0;

            _context.TblUserCategoryMasters.Update(tblUserCategory);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region De-Activate User Category
        public async Task<int> DeActivateUserCategory(int id)
        {
            var userCategory = await _context.TblUserCategoryMasters.Where(x => x.CatId == id && x.CatIsactive == true).FirstOrDefaultAsync();
            if (userCategory == null) return 0;

            userCategory.CatIsactive = false;
            _context.TblUserCategoryMasters.Update(userCategory);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
