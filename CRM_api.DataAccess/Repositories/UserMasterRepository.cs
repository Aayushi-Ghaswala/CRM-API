using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories
{
    public class UserMasterRepository : IUserMasterRepository
    {
        private readonly CRMDbContext _context;
        public UserMasterRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region AddUser
        public async Task<int> AddUser(UserMaster userMaster)
        {
            if (_context.UserMasters.Any(x => x.User_Uname == userMaster.User_Uname))
                throw new Exception("User Name Already Exist");

            _context.UserMasters.Add(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region GetUserDetail By Id
        public async Task<UserMaster> GetUserMasterbyId(int id)
        {
            var user = await _context.UserMasters.Include(x => x.UserCategoryMaster).Include(x => x.UserCategoryMaster).Include(c => c.CountryMaster)
                                                 .Include(s => s.StateMaster).Include(ct => ct.CityMaster).FirstAsync(x => x.User_Id == id);
            ArgumentNullException.ThrowIfNull(user, "User Not Found");

            return user;
        }
        #endregion

        #region UpdateUser Details
        public async Task<int> UpdateUser(UserMaster userMaster)
        {
            _context.UserMasters.Update(userMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get All UserMaster Details
        public async Task<IEnumerable<UserMaster>> GetUsers()
        {
            var users = await _context.UserMasters.Where(x => x.User_IsActive == true).ToListAsync();
            if (users.Count == 0)
                throw new Exception("User Not Found");

            return users;
        }
        #endregion

        #region Get All User Category
        public async Task<IEnumerable<UserCategoryMaster>> GetUserCategories()
        {
            List<UserCategoryMaster> catagories = await _context.UserCategoryMasters.Where(c => c.Cat_IsActive == true).ToListAsync();
            if (catagories.Count == 0)
                throw new Exception("Categories Not Found...");

            return catagories;
        }
        #endregion
    }
}
