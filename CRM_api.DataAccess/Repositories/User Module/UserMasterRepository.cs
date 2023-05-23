﻿using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.User_Module
{
    public class UserMasterRepository : IUserMasterRepository
    {
        private readonly CRMDbContext _context;
        public UserMasterRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get All Users
        public async Task<Response<TblUserMaster>> GetUsers(string filterString, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserMasters.Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();

            if (!string.IsNullOrEmpty(filterString))
            {
                switch (filterString.ToLower())
                {
                    case "client":
                        filterData = filterData.Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer").AsQueryable();
                        break;
                    case "fasttrack":
                        filterData = filterData.Where(x => x.UserFasttrack == true).AsQueryable();
                        break;
                    default:
                        filterData = filterData.AsQueryable();
                        break;
                }
            }

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblUserMaster>(searchingParams);
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var usersResponse = new Response<TblUserMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return usersResponse;
        }
        #endregion

        #region GetUser By Id
        public async Task<TblUserMaster> GetUserMasterbyId(int id)
        {
            var user = await _context.TblUserMasters.Include(x => x.TblUserCategoryMaster).Include(x => x.TblUserCategoryMaster)
                                                    .Include(c => c.TblCountryMaster).Include(s => s.TblStateMaster)
                                                    .Include(ct => ct.TblCityMaster).FirstAsync(x => x.UserId == id);
            ArgumentNullException.ThrowIfNull(user, "User Not Found");

            return user;
        }
        #endregion

        #region Get User Count
        public async Task<Dictionary<string, int>> GetUserCount()
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            var dataCount = _context.TblUserMasters.AsQueryable().Count();
            counts.Add("AllCount", dataCount);

            var clientCount = _context.TblUserMasters.Include(x => x.TblUserCategoryMaster)
                                                    .Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer").AsQueryable().Count();
            counts.Add("ClientCount", clientCount);

            var fastTrackCount = _context.TblUserMasters.Where(x => x.UserFasttrack == true).AsQueryable().Count();
            counts.Add("FastTrackCount", fastTrackCount);

            return counts;
        }
        #endregion

        #region Get All User Category
        public async Task<Response<TblUserCategoryMaster>> GetUserCategories(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserCategoryMasters.AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblUserCategoryMaster>(searchingParams);
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

        #region Get Users By Category Id
        public async Task<Response<TblUserMaster>> GetUsersByCategoryId(int categoryId, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserMasters.Where(x => x.CatId == categoryId).Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .ThenInclude(x => x.TblStateMasters)
                                                    .ThenInclude(x => x.TblCityMasters)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblUserMaster>(searchingParams);
            }
            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var usersResponse = new Response<TblUserMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination()
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return usersResponse;
        }
        #endregion

        #region Get Category By Name
        public async Task<TblUserCategoryMaster> GetCategoryByName(string name)
        {
            var cat = await _context.TblUserCategoryMasters.Where(x => x.CatName == name).FirstOrDefaultAsync();
            ArgumentNullException.ThrowIfNull(cat);

            return cat;
        }
        #endregion

        #region AddUser
        public async Task<int> AddUser(TblUserMaster userMaster)
        {
            if (_context.TblUserMasters.Any(x => x.UserUname == userMaster.UserUname))
                return 0;

            _context.TblUserMasters.Add(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User
        public async Task<int> UpdateUser(TblUserMaster userMaster)
        {
            var user = _context.TblUserMasters.AsNoTracking().Where(x => x.UserId == userMaster.UserId);

            if (user == null) return 0;

            _context.TblUserMasters.Update(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate User
        public async Task<int> DeactivateUser(int id)
        {
            var user = await _context.TblUserMasters.FindAsync(id);
            if (user == null) return 0;

            user.UserIsactive = false;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get UserId By User Pan
        public int GetUserIdByUserPan(string UserPan)
        {
            var user = _context.TblUserMasters.Where(x => x.UserPan == UserPan).FirstOrDefault();
            if(user == null)
                return 0;
            return user.UserId;
        }
        #endregion
    }
}
