using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Investment_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Insvestment_Module
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly CRMDbContext _context;
        public InvestmentRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get InvestmentType
        public async Task<Response<TblInvesmentType>> GetInvestmentType(string? search, SortingParams sortingParams, bool? isActive)
        {
            double? pageCount = 0;
            var investmentTypes = new List<TblInvesmentType>();
            var filterData = investmentTypes.AsQueryable();

            if (search != null)
                filterData = _context.Search<TblInvesmentType>(search).Where(i => isActive == null || i.IsActive == isActive).AsQueryable();
            else
                filterData = _context.TblInvesmentTypes.Where(i => isActive == null || i.IsActive == isActive).AsQueryable();

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var investmentTypeResponse = new Response<TblInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return investmentTypeResponse;
        }
        #endregion

        #region Get InvestmentType by id
        public async Task<TblInvesmentType> GetInvestmentTypeById(int id)
        {
            return await _context.TblInvesmentTypes.Where(i => i.Id == id && i.IsActive == true).FirstOrDefaultAsync();
        }
        #endregion

        #region Get sub InvestmentType
        public async Task<Response<TblSubInvesmentType>> GetSubInvestmentType(string? search, SortingParams sortingParams, bool? isActive)
        {
            double? pageCount = 0;
            var subInvestmentTypes = new List<TblSubInvesmentType>();
            var filterData = subInvestmentTypes.AsQueryable();

            if (search != null)
                filterData = _context.Search<TblSubInvesmentType>(search).Include(i => i.InvesmentType).Where(i => i.IsActive == isActive).AsQueryable();
            else
                filterData = _context.TblSubInvesmentTypes.Include(i => i.InvesmentType).Where(i => isActive == null || i.IsActive == isActive).AsQueryable();

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subInvestmentTypeResponse = new Response<TblSubInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return subInvestmentTypeResponse;
        }
        #endregion

        #region Get InvestmentType by investmentType id
        public async Task<Response<TblSubInvesmentType>> GetSubInvestmentTypeByInvId(int invId, string? search, SortingParams sortingParams)
        {
            double? pageCount = 0;
            var subInvestmentTypes = new List<TblSubInvesmentType>();
            var filterData = subInvestmentTypes.AsQueryable();

            if (search != null)
                filterData = _context.Search<TblSubInvesmentType>(search).Where(s => s.InvesmentTypeId == invId).Include(i => i.InvesmentType).AsQueryable();
            else
                filterData = _context.TblSubInvesmentTypes.Where(s => s.InvesmentTypeId == invId).Include(i => i.InvesmentType).AsQueryable();

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subInvestmentTypeResponse = new Response<TblSubInvesmentType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return subInvestmentTypeResponse;
        }
        #endregion

        #region Get SubsubInvestmentType
        public async Task<Response<TblSubsubInvType>> GetSubsubInvestmentType(string? search, SortingParams sortingParams, bool? isActive)
        {
            double? pageCount = 0;
            var subSubInvestmentTypes = new List<TblSubsubInvType>();
            var filterData = subSubInvestmentTypes.AsQueryable();

            if (search != null)
                filterData = _context.Search<TblSubsubInvType>(search).Include(i => i.TblSubInvesmentType).ThenInclude(i => i.InvesmentType).Where(i => isActive == null || isActive == null || i.IsActive == isActive).AsQueryable();
            else
                filterData = _context.TblSubsubInvTypes.Include(i => i.TblSubInvesmentType).ThenInclude(i => i.InvesmentType).Where(i => isActive == null || i.IsActive == isActive).AsQueryable();

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subSubInvestmentTypeResponse = new Response<TblSubsubInvType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return subSubInvestmentTypeResponse;
        }
        #endregion

        #region Get SubsubInvestmentType by subInvestmentType id
        public async Task<Response<TblSubsubInvType>> GetSubsubInvestmentTypeBySubInvId(int subInvId, string? search, SortingParams sortingParams)
        {
            double? pageCount = 0;
            var subSubInvestmentTypes = new List<TblSubsubInvType>();
            var filterData = subSubInvestmentTypes.AsQueryable();

            if (search != null)
                filterData = _context.Search<TblSubsubInvType>(search).Where(i => i.SubInvTypeId == subInvId).Include(i => i.TblSubInvesmentType).AsQueryable();
            else
                filterData = _context.TblSubsubInvTypes.Where(i => i.SubInvTypeId == subInvId).Include(i => i.TblSubInvesmentType).AsQueryable();

            pageCount = Math.Ceiling(filterData.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var subSubInvestmentTypeResponse = new Response<TblSubsubInvType>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            return subSubInvestmentTypeResponse;
        }
        #endregion

        #region Get All SubInvestment Types
        public async Task<List<TblSubInvesmentType>> GetAllSubInvTypes()
        {
            var subInvTypes = await _context.TblSubInvesmentTypes.Where(x => x.IsActive == true).ToListAsync();

            return subInvTypes;
        }
        #endregion

        #region Add InvestmentType
        public async Task<int> AddInvestmentType(TblInvesmentType tblInvesmentType)
        {
            if (_context.TblInvesmentTypes.Any(i => i.InvestmentName.ToLower().Equals(tblInvesmentType.InvestmentName.ToLower()) && i.IsActive == tblInvesmentType.IsActive))
                return 0;
            await _context.TblInvesmentTypes.AddAsync(tblInvesmentType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add SubInvestmentType
        public async Task<int> AddSubInvestmentType(TblSubInvesmentType tblSubInvesment)
        {
            if (_context.TblSubInvesmentTypes.Any(i => i.InvestmentType.ToLower().Equals(tblSubInvesment.InvestmentType.ToLower()) && i.InvesmentTypeId == tblSubInvesment.InvesmentTypeId && i.IsActive == tblSubInvesment.IsActive))
                return 0;
            await _context.TblSubInvesmentTypes.AddAsync(tblSubInvesment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add SubsubInvestmentType
        public async Task<int> AddSubsubInvestmentType(TblSubsubInvType tblSubsubInvType)
        {
            if (_context.TblSubsubInvTypes.Any(i => i.SubInvType.ToLower().Equals(tblSubsubInvType.SubInvType.ToLower()) && i.SubInvTypeId == tblSubsubInvType.SubInvTypeId && i.IsActive == tblSubsubInvType.IsActive))
                return 0;
            await _context.TblSubsubInvTypes.AddAsync(tblSubsubInvType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update InvestmentType
        public async Task<int> UpdateInvestmentType(TblInvesmentType tblInvesmentType)
        {
            if (_context.TblInvesmentTypes.Any(i => i.InvestmentName.ToLower().Equals(tblInvesmentType.InvestmentName.ToLower()) && i.Id != tblInvesmentType.Id))
                return 0;
            _context.TblInvesmentTypes.Update(tblInvesmentType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update SubInvestmentType
        public async Task<int> UpdateSubInvestmentType(TblSubInvesmentType tblSubInvesmentType)
        {
            if (_context.TblSubInvesmentTypes.Any(i => i.InvestmentType.ToLower().Equals(tblSubInvesmentType.InvestmentType.ToLower()) && i.Id != tblSubInvesmentType.Id))
                return 0;
            _context.TblSubInvesmentTypes.Update(tblSubInvesmentType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update SubsubInvestmentType
        public async Task<int> UpdateSubsubInvestmentType(TblSubsubInvType tblSubsubInvType)
        {
            if (_context.TblSubsubInvTypes.Any(i => i.SubInvType.ToLower().Equals(tblSubsubInvType.SubInvType.ToLower()) && i.Id != tblSubsubInvType.Id))
                return 0;
            _context.TblSubsubInvTypes.Update(tblSubsubInvType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactive InvestmentType
        public async Task<int> DeactiveInvestmentType(int id)
        {
            var investment = await GetInvestmentTypeById(id);
            if (investment == null) return 0;

            var subInvTypesCount = _context.TblSubInvesmentTypes.Where(i => i.InvesmentTypeId == id && i.IsActive == true).Count();
            if (subInvTypesCount > 0) return 0;

            investment.IsActive = false;
            _context.TblInvesmentTypes.Update(investment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactive SubInvestmentType
        public async Task<int> DeactiveSubInvestmentType(int id)
        {
            var subInv = await _context.TblSubInvesmentTypes.FirstOrDefaultAsync(i => i.Id == id);
            if (subInv == null) return 0;

            var subSubtypesCount = _context.TblSubsubInvTypes.Where(i => i.SubInvTypeId == id && i.IsActive == true).Count();
            if (subSubtypesCount > 0) return 0;

            subInv.IsActive = false;
            _context.TblSubInvesmentTypes.Update(subInv);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactive SubsubInvestmentType
        public async Task<int> DeactiveSubsubInvestmentType(int id)
        {
            var subSubInv = await _context.TblSubsubInvTypes.FirstOrDefaultAsync(i => i.Id == id);
            if (subSubInv == null) return 0;

            subSubInv.IsActive = false;
            _context.TblSubsubInvTypes.Update(subSubInv);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
