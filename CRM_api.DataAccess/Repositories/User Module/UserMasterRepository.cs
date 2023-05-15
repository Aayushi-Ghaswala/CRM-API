using CRM_api.DataAccess.Context;
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

        #region GetUserDetail By Id
        public async Task<TblUserMaster> GetUserMasterbyId(int id)
        {
            var user = await _context.TblUserMasters.Include(x => x.TblUserCategoryMaster).Include(x => x.TblUserCategoryMaster)
                                                    .Include(c => c.TblCountryMaster).Include(s => s.TblStateMaster)
                                                    .Include(ct => ct.TblCityMaster).FirstAsync(x => x.UserId == id);
            ArgumentNullException.ThrowIfNull(user, "User Not Found");

            return user;
        }
        #endregion

        #region Get All TblUserMaster Details
        public async Task<Response<TblUserMaster>> GetUsers(int page, string search, string sortOn)
        {
            double pageCount = 0;
            var users = new List<TblUserMaster>();
            float pageResult = 10f;
            if (search is not null)
            {
                pageCount = Math.Ceiling(_context.TblUserMasters.Where(x => x.UserIsactive == true && (x.UserName.ToLower().Contains(search.ToLower())
                    || x.UserUname.ToLower().Contains(search.ToLower()) || x.UserEmail.ToLower().Contains(search.ToLower()))).Count() / pageResult);

                users = await _context.TblUserMasters.Where(x => x.UserIsactive == true && x.UserName.ToLower().Contains(search.ToLower())
                    || x.UserUname.ToLower().Contains(search.ToLower()) || x.UserEmail.ToLower().Contains(search.ToLower())).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).ToListAsync();


            }
            else if(sortOn is not null)
            {
                pageCount = Math.Ceiling(_context.TblUserMasters.Where(x => x.UserIsactive == true).Count() / pageResult);

                 switch(sortOn)
                 {
                    case "userName":
                        users = await _context.TblUserMasters.Where(x => x.UserIsactive == true).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).OrderByDescending(x => x.UserName).ToListAsync();
                        break;

                    case "userUname":
                        users = await _context.TblUserMasters.Where(x => x.UserIsactive == true).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).OrderByDescending(x => x.UserUname).ToListAsync();
                        break;

                    case "userDoj":
                        users = await _context.TblUserMasters.Where(x => x.UserIsactive == true).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).OrderByDescending(x => x.UserDoj).ToListAsync();
                        break;

                    case "userEmail":
                        users = await _context.TblUserMasters.Where(x => x.UserIsactive == true).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).OrderByDescending(x => x.UserEmail).ToListAsync();
                        break;
                 }
            }
            else
            {
                pageCount = Math.Ceiling(_context.TblUserMasters.Where(x => x.UserIsactive == true).Count() / pageResult);

                users = await _context.TblUserMasters.Where(x => x.UserIsactive == true).Skip((page - 1) * (int)pageResult)
                                                         .Take((int)pageResult).Include(x => x.TblUserCategoryMaster).Include(x => x.TblCountryMaster)
                                                         .Include(x => x.TblStateMaster).Include(x => x.TblCityMaster).Include(x => x.ParentName)
                                                         .Include(x => x.SponserName).ToListAsync();
            }

            var usersResponse = new Response<TblUserMaster>()
            {
                Values = users,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return usersResponse;
        }
        #endregion

        #region Get All User Category
        public async Task<Response<TblUserCategoryMaster>> GetUserCategories(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblUserCategoryMasters.Count() / pageResult);

            List<TblUserCategoryMaster> catagories = await _context.TblUserCategoryMasters.Where(c => c.CatIsactive == true).Skip((page - 1) * (int)pageResult)
                                                     .Take((int)pageResult).ToListAsync();

            var categoriesResponse = new Response<TblUserCategoryMaster>()
            {
                Values = catagories,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return categoriesResponse;
        }
        #endregion

        #region Get Category Id By Name
        public async Task<int> GetCategoryIdByName(string name)
        {
            var cat = await _context.TblUserCategoryMasters.Where(x => x.CatName == name).FirstOrDefaultAsync();
            ArgumentNullException.ThrowIfNull(cat);

            return cat.CatId;
        }
        #endregion

        #region Get Users By Category Id
        public async Task<Response<TblUserMaster>> GetUsersByCategoryId(int page, int catId)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblUserMasters.Where(x => x.UserIsactive == true && x.CatId == catId).Count() / pageResult);

            var users = await _context.TblUserMasters.Where(x => x.UserIsactive == true && x.CatId == catId).Skip((page - 1) * (int)pageResult)
                                                     .Take((int)pageResult).ToListAsync();

            var usersResponse = new Response<TblUserMaster>()
            {
                Values = users,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return usersResponse;
        }
        #endregion

        #region AddUser
        public async Task<int> AddUser(TblUserMaster userMaster)
        {
            if (_context.TblUserMasters.Any(x => x.UserUname == userMaster.UserUname))
                throw new Exception("User Name Already Exist");

            _context.TblUserMasters.Add(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateUser Details
        public async Task<int> UpdateUser(TblUserMaster userMaster)
        {
            var user = await _context.TblUserMasters.FindAsync(userMaster.UserId);

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
    }
}
