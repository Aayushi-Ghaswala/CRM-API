﻿using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CRM_api.DataAccess.Repositories.Business_Module.Fasttrack_Module
{
    public class FasttrackRepository : IFasttrackRepository
    {
        private readonly CRMDbContext _context;

        public FasttrackRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get fasttrack category of user
        public async Task<string> GetUserFasttrackCategory(int userId)
        {
            var userCategory = "";
            var allUsers = await _context.TblUserMasters.Where(u => u.UserIsactive == true).ToListAsync();
            var fasttrack_scheme_master = await _context.TblFasttrackSchemeMasters.ToListAsync();
            fasttrack_scheme_master.Reverse();
            var basicScheme = fasttrack_scheme_master.Last();

            var currUser = allUsers.Where(u => u.UserId == userId).FirstOrDefault();

            var totalContacts = allUsers.Where(u => u.UserParentid == currUser.UserId).Count();

            //getting curr new joinee user's parent's wbc and fasttrack clients
            var totalWbcCount = allUsers.Where(u => u.UserParentid == currUser.UserId && u.UserWbcActive == true && u.UserFasttrack == false).Count();
            var totalFasttrackCount = allUsers.Where(u => u.UserParentid == currUser.UserId && u.UserFasttrack == true).Count();
           
            if (totalWbcCount > basicScheme.NoOfNonFasttrackClients && totalFasttrackCount > basicScheme.NoOfFasttrackClients)
            {
                userCategory = fasttrack_scheme_master.Where(x => x.NoOfFasttrackClients <= totalFasttrackCount && x.NoOfNonFasttrackClients <= totalWbcCount).First().Level;
            }
            else
            {
                userCategory = fasttrack_scheme_master.Last().Level;
            }
            return userCategory;
        }
        #endregion

        #region Update fasttrack levels commission
        public async Task<int> UpdateFasttrackLevelsCommission(TblFasttrackLevelCommission tblFasttrackLevelCommission)
        {
            _context.TblFasttrackLevelCommissions.Update(tblFasttrackLevelCommission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update fasttrack scheme
        public async Task<int> UpdateFasttrackScheme(TblFasttrackSchemeMaster tblFasttrackSchemeMaster)
        {
            _context.TblFasttrackSchemeMasters.Update(tblFasttrackSchemeMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
