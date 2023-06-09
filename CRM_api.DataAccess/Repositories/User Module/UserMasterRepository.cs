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
        public async Task<Response<TblUserMaster>> GetUsers(string filterString, string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserMasters.Where(x => x.UserIsactive == true).Include(x => x.TblUserCategoryMaster)
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
                        filterData = filterData.Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer" && x.UserIsactive == true).AsQueryable();
                        break;
                    case "fasttrack":
                        filterData = filterData.Where(x => x.UserFasttrack == true && x.UserIsactive == true).AsQueryable();
                        break;
                    case "employee":
                        filterData = filterData.Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "employee" && x.UserIsactive == true).AsQueryable();
                        break;
                    default:
                        break;
                }
            }

            if (search != null)
            {
                if (!string.IsNullOrEmpty(filterString))
                {
                    switch (filterString.ToLower())
                    {
                        case "client":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer" && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        case "fasttrack":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.UserFasttrack == true && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        case "employee":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "employee" && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    filterData = _context.Search<TblUserMaster>(search).Where(x => x.UserIsactive == true)
                                                        .Include(x => x.TblUserCategoryMaster)
                                                        .Include(x => x.TblCountryMaster)
                                                        .Include(x => x.TblStateMaster)
                                                        .Include(x => x.TblCityMaster)
                                                        .Include(x => x.ParentName)
                                                        .Include(x => x.SponserName).AsQueryable();
                }
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
                                                    .Include(ct => ct.TblCityMaster).FirstAsync(x => x.UserId == id && x.UserIsactive != false);
            ArgumentNullException.ThrowIfNull(user, "User Not Found");

            return user;
        }
        #endregion

        #region Get User Count
        public async Task<Dictionary<string, int>> GetUserCount()
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            var dataCount = _context.TblUserMasters.Where(x => x.UserIsactive == true).AsQueryable().Count();
            counts.Add("AllCount", dataCount);

            var clientCount = _context.TblUserMasters.Include(x => x.TblUserCategoryMaster)
                                                    .Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer" && x.UserIsactive == true).AsQueryable().Count();
            counts.Add("ClientCount", clientCount);

            var fastTrackCount = _context.TblUserMasters.Where(x => x.UserFasttrack == true && x.UserIsactive == true).AsQueryable().Count();
            counts.Add("FastTrackCount", fastTrackCount);

            var employeeCount = _context.TblUserMasters.Include(x => x.TblUserCategoryMaster)
                                                    .Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "employee" && x.UserIsactive == true).AsQueryable().Count();
            counts.Add("EmployeeCount", employeeCount);

            return counts;
        }
        #endregion

        #region Get All User Category
        public async Task<Response<TblUserCategoryMaster>> GetUserCategories(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserCategoryMasters.Where(x => x.CatIsactive != false).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserCategoryMaster>(search).Where(x => x.CatIsactive != false).AsQueryable();
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
        public async Task<Response<TblUserMaster>> GetUsersByCategoryId(int categoryId, string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .ThenInclude(x => x.TblStateMasters)
                                                    .ThenInclude(x => x.TblCityMasters)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserMaster>(search).Where(x => x.CatId == categoryId && x.UserIsactive != false)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .ThenInclude(x => x.TblStateMasters)
                                                    .ThenInclude(x => x.TblCityMasters)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
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

        #region Check Pan or Aadhar Exist
        public int PanOrAadharExist(int? id, string? pan, string? aadhar)
        {
            if (id is null)
            {
                if (pan is not null)
                {
                    if (_context.TblUserMasters.Any(x => x.UserPan == pan))
                        return 0;
                }
                else if (aadhar is not null)
                {
                    if (_context.TblUserMasters.Any(x => x.UserAadhar == aadhar))
                        return 0;
                }
            }
            else
            {
                if (pan is not null)
                {
                    if (_context.TblUserMasters.Any(x => x.UserPan == pan && x.UserId != id))
                        return 0;
                }
                else if (aadhar is not null)
                {
                    if (_context.TblUserMasters.Any(x => x.UserAadhar == aadhar && x.UserId != id))
                        return 0;
                }
            }
            return 1;
        }
        #endregion

        #region Get All Users For CSV
        public async Task<List<TblUserMaster>> GetUsersForCSV(string filterString, string search, SortingParams sortingParams)
        {
            var filterData = _context.TblUserMasters.Where(x => x.UserIsactive == true).Include(x => x.TblUserCategoryMaster)
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
                        filterData = filterData.Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer" && x.UserIsactive == true).AsQueryable();
                        break;
                    case "fasttrack":
                        filterData = filterData.Where(x => x.UserFasttrack == true && x.UserIsactive == true).AsQueryable();
                        break;
                    case "employee":
                        filterData = filterData.Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "employee" && x.UserIsactive == true).AsQueryable();
                        break;
                    default:
                        break;
                }
            }

            if (search != null)
            {
                if (!string.IsNullOrEmpty(filterString))
                {
                    switch (filterString.ToLower())
                    {
                        case "client":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "customer" && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        case "fasttrack":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.UserFasttrack == true && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        case "employee":
                            filterData = _context.Search<TblUserMaster>(search).Where(x => x.TblUserCategoryMaster.CatName.ToLower() == "employee" && x.UserIsactive == true)
                                                    .Include(x => x.TblUserCategoryMaster)
                                                    .Include(x => x.TblCountryMaster)
                                                    .Include(x => x.TblStateMaster)
                                                    .Include(x => x.TblCityMaster)
                                                    .Include(x => x.ParentName)
                                                    .Include(x => x.SponserName).AsQueryable();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    filterData = _context.Search<TblUserMaster>(search).Where(x => x.UserIsactive == true)
                                                        .Include(x => x.TblUserCategoryMaster)
                                                        .Include(x => x.TblCountryMaster)
                                                        .Include(x => x.TblStateMaster)
                                                        .Include(x => x.TblCityMaster)
                                                        .Include(x => x.ParentName)
                                                        .Include(x => x.SponserName).AsQueryable();
                }
            }

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending).ToList();

            return sortedData;
        }
        #endregion

        #region AddUser
        public async Task<TblUserMaster> AddUser(TblUserMaster userMaster)
        {
            if (userMaster.UserPan is not null)
            {
                if (_context.TblUserMasters.Any(x => x.UserPan == userMaster.UserPan))
                    return null;
            }
            if (userMaster.UserAadhar is not null)
            {
                if (_context.TblUserMasters.Any(x => x.UserAadhar == userMaster.UserAadhar))
                    return null;
            }


            _context.TblUserMasters.Add(userMaster);
            await _context.SaveChangesAsync();
            return userMaster;
        }
        #endregion

        #region Update User
        public async Task<int> UpdateUser(TblUserMaster userMaster)
        {
            var user = await _context.TblUserMasters.AsNoTracking().Where(x => x.UserId == userMaster.UserId).FirstAsync();

            if (user == null) return 0;
            if (userMaster.UserPan is not null)
            {
                if (_context.TblUserMasters.Any(x => x.UserPan == userMaster.UserPan && x.UserId != user.UserId))
                    return 0;
            }
            if (userMaster.UserAadhar is not null)
            {
                if (_context.TblUserMasters.Any(x => x.UserAadhar == userMaster.UserAadhar && x.UserId != user.UserId))
                    return 0;
            }

            if (user.UserFasttrack == false)
            {
                if (userMaster.UserFasttrack == true)
                    userMaster.FastTrackActivationDate = DateTime.Now;
            }

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
            if (user == null)
                return 0;
            return user.UserId;
        }
        #endregion

        #region Get User By User Pan
        public TblUserMaster GetUserByUserPan(string UserPan)
        {
            var user = _context.TblUserMasters.Where(x => x.UserPan == UserPan).FirstOrDefault();
            return user;
        }
        #endregion

        #region Get User By Email
        public async Task<TblUserMaster> GetUserByEmail(string email)
        {
            var user = await _context.TblUserMasters.Where(x => x.UserEmail.ToLower().Equals(email.ToLower()) && x.UserIsactive == true)
                                                    .Include(x => x.TblRoleAssignments.Where(x => x.IsDeleted == false))
                                                        .ThenInclude(x => x.TblRoleMaster)
                                                            .ThenInclude(x => x.TblRolePermissions.Where(x => x.IsDeleted == false)).ThenInclude(x => x.TblModuleMaster)
                                                    .Include(x => x.TblUserCategoryMaster).FirstOrDefaultAsync();

            return user;
        }
        #endregion

        #region Get User By Parent Id
        public async Task<List<TblUserMaster>> GetUserByParentId(int? userId, DateTime date)
        {
            var users = new List<TblUserMaster>();

            if (userId is not null)
            {
                users = await _context.TblUserMasters.Where(x => x.UserParentid == userId && x.UserDoj.Value.Month >= date.Month && x.UserDoj.Value.Year >= date.Year
                                                     && x.UserDoj.Value.Month <= DateTime.Now.Month && x.UserDoj.Value.Year <= DateTime.Now.Year && x.UserIsactive == true).ToListAsync();
            }
            else
            {
                users = await _context.TblUserMasters.Where(x => x.UserDoj.Value.Month >= date.Month && x.UserDoj.Value.Year >= date.Year
                                                     && x.UserDoj.Value.Month <= DateTime.Now.Month && x.UserDoj.Value.Year <= DateTime.Now.Year && x.UserIsactive == true).ToListAsync();
            }

            return users;
        }
        #endregion
    }
}