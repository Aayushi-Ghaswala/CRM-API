using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.WBC_Mall_Module
{
    public class MallCategoryRepository : IMallCategoryRepository
    {
        private readonly CRMDbContext _context;

        public MallCategoryRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get WBC Mall Categories
        public async Task<Response<TblWbcMallCategory>> GetMallCategories(string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblWbcMallCategory>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblWbcMallCategory>(search).Where(x => x.CatActive == true).AsQueryable();
            }
            else
            {
                filterData = _context.TblWbcMallCategories.Where(x => x.CatActive == true).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            // Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var categoriesResponse = new Response<TblWbcMallCategory>()
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

        #region Get Mall Category By Id
        public async Task<TblWbcMallCategory> GetMallCategoryById(int id)
        {
            var mallCategory = await _context.TblWbcMallCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CatId == id && x.CatActive == true);
            if (mallCategory == null) return null;

            return mallCategory;
        }
        #endregion

        #region Add Mall Category
        public async Task<TblWbcMallCategory> AddMallCategory(TblWbcMallCategory wbcMallCategory)
        {
            if (_context.TblWbcMallCategories.Any(x => x.CatName.ToLower().Equals(wbcMallCategory.CatName.ToLower()) && x.CatActive == true))
                return null;

            _context.TblWbcMallCategories.Add(wbcMallCategory);
            await _context.SaveChangesAsync();
            return wbcMallCategory;
        }
        #endregion

        #region Update Mall Category
        public async Task<int> UpdateMallCategory(TblWbcMallCategory wbcMallCategory)
        {
            _context.TblWbcMallCategories.Update(wbcMallCategory);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region De-Activate Mall Category
        public async Task<int> DeActivateMallCategory(int id)
        {
            var mallCategory = await _context.TblWbcMallCategories.AsNoTracking().FirstOrDefaultAsync(x => x.CatId == id && x.CatActive == true);
            if (mallCategory == null) return 0;
            if (_context.TblWbcMallProducts.Any(x => x.ProdCatId == id)) return 2;

            mallCategory.CatActive = false;
            _context.TblWbcMallCategories.Update(mallCategory);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
