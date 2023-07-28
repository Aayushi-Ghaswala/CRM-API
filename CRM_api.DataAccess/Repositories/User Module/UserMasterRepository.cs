using CRM_api.DataAccess.Context;
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

            var filterData = new List<TblUserMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserMaster>(search).Where(x => (filterString == null || (filterString.ToLower() == "client" && x.TblUserCategoryMaster.CatName.ToLower() == "customer")
                                       || (filterString.ToLower() == "fasttrack" && x.UserFasttrack == true)
                                       || (filterString.ToLower() == "employee" && x.TblUserCategoryMaster.CatName.ToLower() == "employee"))
                                       && x.UserIsactive == true)
                                        .Include(x => x.TblUserCategoryMaster)
                                        .Include(x => x.TblCountryMaster)
                                        .Include(x => x.TblStateMaster)
                                        .Include(x => x.TblCityMaster)
                                        .Include(x => x.ParentName).AsQueryable();
            }
            else
            {
                filterData = _context.TblUserMasters.Where(x => (filterString == null || (filterString.ToLower() == "client" && x.TblUserCategoryMaster.CatName.ToLower() == "customer")
                                                   || (filterString.ToLower() == "fasttrack" && x.UserFasttrack == true)
                                                   || (filterString.ToLower() == "employee" && x.TblUserCategoryMaster.CatName.ToLower() == "employee"))
                                                   && x.UserIsactive == true).Include(x => x.TblUserCategoryMaster)
                                                   .Include(x => x.TblCountryMaster)
                                                   .Include(x => x.TblStateMaster)
                                                   .Include(x => x.TblCityMaster)
                                                   .Include(x => x.ParentName).AsQueryable();
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
        public async Task<TblUserMaster> GetUserMasterbyId(int id, int? month = null, int? year = null, bool isCurrent = false)
        {
            var user = new TblUserMaster();

            if (isCurrent)
            {
                user = await _context.TblUserMasters.Include(x => x.TblUserCategoryMaster).Include(x => x.TblUserCategoryMaster)
                                                    .Include(c => c.TblCountryMaster).Include(s => s.TblStateMaster)
                                                    .Include(ct => ct.TblCityMaster).Include(m => m.TblMgaindetails)
                                                    .Include(mf => mf.TblMftransactions).Include(l => l.TblLoanmasters)
                                                    .Include(ins => ins.TblInsuranceclients).AsNoTracking().FirstAsync(x => x.UserId == id && x.UserIsactive != false);
            }
            else if (month is not null && year is not null)
            {
                user = await _context.TblUserMasters.Where(x => x.UserId == id && x.UserIsactive != false)
                                                        .Include(x => x.TblMgaindetails)
                                                        .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                            && y.Date.Value.Month == month && y.Date.Value.Year == year))
                                                        .Include(x => x.TblLoanmasters)
                                                        .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= month && y.InsDuedate.Value.Year >= year
                                                                                                      && y.InsStartdate.Value.Month <= month && y.InsStartdate.Value.Year <= year))
                                                        .AsNoTracking().FirstAsync();
            }
            else
            {
                user = await _context.TblUserMasters.Where(x => x.UserId == id && x.UserIsactive != false)
                                                    .Include(x => x.TblMgaindetails)
                                                    .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                            && y.Date.Value.Month == DateTime.Now.Month && y.Date.Value.Year == DateTime.Now.Year))
                                                    .Include(x => x.TblLoanmasters)
                                                    .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= DateTime.Now.Month && y.InsDuedate.Value.Year >= DateTime.Now.Year
                                                                                                        && y.InsStartdate.Value.Month <= DateTime.Now.Month && y.InsStartdate.Value.Year <= DateTime.Now.Year))
                                                    .AsNoTracking().FirstAsync();
            }

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

        #region Get Users By Category Id
        public async Task<List<TblUserMaster>> GetUsersByCategoryId(int categoryId, int? month, int? year, string search, bool isCurrent = false)
        {
            var filterData = new List<TblUserMaster>();
            if (isCurrent)
            {
                if (search is not null)
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false
                                                               && x.UserName.ToLower().Contains(search.ToLower()))
                                                              .Include(x => x.TblMgaindetails)
                                                              .Include(x => x.TblMftransactions)
                                                              .Include(x => x.TblLoanmasters)
                                                              .Include(x => x.TblInsuranceclients)
                                                              .ToListAsync();
                else
                {
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false)
                                                              .Include(x => x.TblMgaindetails)
                                                              .Include(x => x.TblMftransactions)
                                                              .Include(x => x.TblLoanmasters)
                                                              .Include(x => x.TblInsuranceclients)
                                                              .ToListAsync();
                }
            }
            else if (search != null)
            {
                if (month is not null && year is not null)
                {
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false
                                                         && x.UserName.ToLower().Contains(search.ToLower()))
                                                        .Include(x => x.TblMgaindetails)
                                                        .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                            && y.Date.Value.Month == month && y.Date.Value.Year == year))
                                                        .Include(x => x.TblLoanmasters)
                                                        .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= month && y.InsDuedate.Value.Year >= year
                                                                                                      && y.InsStartdate.Value.Month <= month && y.InsStartdate.Value.Year <= year))
                                                        .ToListAsync();
                    filterData = filterData.OrderBy(x => x.UserName).ToList();
                }
                else
                {
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false
                                                             && x.UserName.ToLower().Contains(search.ToLower()))
                                                            .Include(x => x.TblMgaindetails)
                                                            .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                                && y.Date.Value.Month == DateTime.Now.Month && y.Date.Value.Year == DateTime.Now.Year))
                                                            .Include(x => x.TblLoanmasters)
                                                            .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= DateTime.Now.Month && y.InsDuedate.Value.Year >= DateTime.Now.Year
                                                                                                        && y.InsStartdate.Value.Month <= DateTime.Now.Month && y.InsStartdate.Value.Year <= DateTime.Now.Year))
                                                            .ToListAsync();
                    filterData = filterData.OrderBy(x => x.UserName).ToList();
                }
            }
            else
            {
                if (month is not null && year is not null)
                {
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false)
                                                        .Include(x => x.TblMgaindetails)
                                                        .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                            && y.Date.Value.Month == month && y.Date.Value.Year == year))
                                                        .Include(x => x.TblLoanmasters)
                                                        .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= month && y.InsDuedate.Value.Year >= year
                                                                                                      && y.InsStartdate.Value.Month <= month && y.InsStartdate.Value.Year <= year))
                                                        .ToListAsync();
                    filterData = filterData.OrderBy(x => x.UserName).ToList();
                }
                else
                {
                    filterData = await _context.TblUserMasters.Where(x => x.CatId == categoryId && x.UserIsactive != false)
                                                        .Include(x => x.TblMgaindetails)
                                                        .Include(x => x.TblMftransactions.Where(y => (y.Transactiontype == "PIP" || y.Transactiontype == "PIP (SIP)")
                                                                            && y.Date.Value.Month == DateTime.Now.Month && y.Date.Value.Year == DateTime.Now.Year))
                                                        .Include(x => x.TblLoanmasters)
                                                        .Include(x => x.TblInsuranceclients.Where(y => y.InsDuedate.Value.Month >= DateTime.Now.Month && y.InsDuedate.Value.Year >= DateTime.Now.Year
                                                                                                        && y.InsStartdate.Value.Month <= DateTime.Now.Month && y.InsStartdate.Value.Year <= DateTime.Now.Year))
                                                        .ToListAsync();
                    filterData = filterData.OrderBy(x => x.UserName).ToList();
                }
            }

            return filterData;
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
            if (_context.TblUserMasters.Any(x => (id == null || x.UserId == id) && ((pan != null && x.UserPan == pan) || (aadhar != null && x.UserAadhar == aadhar))))
                return 0;
            return 1;
        }
        #endregion

        #region Get All Users For CSV
        public async Task<List<TblUserMaster>> GetUsersForCSV(string filterString, string search, SortingParams sortingParams)
        {
            var filterData = new List<TblUserMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblUserMaster>(search).Where(x => (filterString == null || (filterString.ToLower() == "client" && x.TblUserCategoryMaster.CatName.ToLower() == "customer")
                           || (filterString.ToLower() == "fasttrack" && x.UserFasttrack == true)
                           || (filterString.ToLower() == "employee" && x.TblUserCategoryMaster.CatName.ToLower() == "employee"))
                           && x.UserIsactive == true)
                            .Include(x => x.TblUserCategoryMaster)
                            .Include(x => x.TblCountryMaster)
                            .Include(x => x.TblStateMaster)
                            .Include(x => x.TblCityMaster)
                            .Include(x => x.ParentName).AsQueryable();
            }
            else
            {
                filterData = _context.TblUserMasters.Where(x => (filterString == null || (filterString.ToLower() == "client" && x.TblUserCategoryMaster.CatName.ToLower() == "customer")
                                   || (filterString.ToLower() == "fasttrack" && x.UserFasttrack == true)
                                   || (filterString.ToLower() == "employee" && x.TblUserCategoryMaster.CatName.ToLower() == "employee"))
                                   && x.UserIsactive == true)
                                    .Include(x => x.TblUserCategoryMaster)
                                    .Include(x => x.TblCountryMaster)
                                    .Include(x => x.TblStateMaster)
                                    .Include(x => x.TblCityMaster)
                                    .Include(x => x.ParentName).AsQueryable();
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
            var users = await _context.TblUserMasters.Where(x => (userId == null && x.UserParentid == userId) && x.UserDoj.Value.Month >= date.Month && x.UserDoj.Value.Year >= date.Year
                                                 && x.UserDoj.Value.Month <= DateTime.Now.Month && x.UserDoj.Value.Year <= DateTime.Now.Year && x.UserIsactive == true).ToListAsync();

            return users;
        }
        #endregion
    }
}