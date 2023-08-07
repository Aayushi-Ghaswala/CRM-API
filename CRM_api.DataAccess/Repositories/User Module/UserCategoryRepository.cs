using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
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
