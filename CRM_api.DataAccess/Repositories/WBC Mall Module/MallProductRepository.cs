using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.WBC_Mall_Module
{
    public class MallProductRepository : IMallProductRepository
    {
        private readonly CRMDbContext _context;

        public MallProductRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get Mall Products
        public async Task<Response<TblWbcMallProduct>> GetMallProducts(int? catId, string? filterString, string? search, SortingParams sortingParams)
        {
            double pageCount = 0;
            var filterData = new List<TblWbcMallProduct>().AsQueryable();

            if (search is not null)
            {
                filterData = _context.Search<TblWbcMallProduct>(search).Where(x => (catId == null || x.ProdCatId == catId) && (filterString == null 
                                                                       || x.ManagedBy.ToLower().Equals(filterString.ToLower()))).Include(x => x.TblWbcMallCategory)
                                                                       .Include(x => x.TblProductImgs).AsQueryable();
            }
            else
            {
                filterData = _context.TblWbcMallProducts.Where(x => (catId == null || x.ProdCatId == catId) && (filterString == null
                                                                       || x.ManagedBy.ToLower().Equals(filterString.ToLower()))).Include(x => x.TblWbcMallCategory)
                                                                       .Include(x => x.TblProductImgs).AsQueryable();
            }

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var productsResponse = new Response<TblWbcMallProduct>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return productsResponse;
        }
        #endregion

        #region Get Mall Product By Id
        public async Task<TblWbcMallProduct> GetMallProductById(int id)
        {
            var product = await _context.TblWbcMallProducts.Where(x => x.ProdId == id).AsNoTracking().Include(x => x.TblWbcMallCategory).Include(x => x.TblProductImgs)
                                                           .FirstOrDefaultAsync();

            if (product == null) return null;

            return product;
        }
        #endregion

        #region Add Mall Product
        public async Task<TblWbcMallProduct> AddMallProduct(TblWbcMallProduct tblWbcMallProduct)
        {
            _context.TblWbcMallProducts.Add(tblWbcMallProduct);
            await _context.SaveChangesAsync();
            return tblWbcMallProduct;
        }
        #endregion

        #region Update Mall Product
        public async Task<int> UpdateMallProduct(List<TblWbcMallProduct> tblWbcMallProducts)
        {
            _context.TblWbcMallProducts.UpdateRange(tblWbcMallProducts);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Delete Product Image
        public async Task<int> DeleteProductImage(int id)
        {
            var productImage = await _context.TblProductImgs.Where(x => x.Id == id && x.Isdeleted != true).FirstOrDefaultAsync();
            if (productImage is null) return 0;

            if (File.Exists(productImage.Img))
            {
                File.Delete(productImage.Img);
            }

            _context.TblProductImgs.Remove(productImage);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
