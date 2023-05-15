using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.User_Module
{
    public class RoleMasterRepository : IRoleMasterRepository
    {
        private readonly CRMDbContext _context;

        public RoleMasterRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Add Role
        public async Task<int> AddRole(TblRoleMaster roleMaster)
        {
            await _context.TblRoleMasters.AddAsync(roleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add RolePermission
        public async Task<int> AddRolePermission(TblRolePermission rolePermission)
        {
            await _context.TblRolePermissions.AddAsync(rolePermission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add UserRoleAssignment
        public async Task<int> AddUserRoleAssignment(TblRoleAssignment userRoleAssignment)
        {
            await _context.TblRoleAssignments.AddAsync(userRoleAssignment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role
        public async Task<int> UpdateRole(TblRoleMaster roleMaster)
        {
            var role = await _context.TblRoleMasters.FindAsync(roleMaster.RoleId);

            if (role == null) return 0;

            _context.TblRoleMasters.Update(roleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role Permission
        public async Task<int> UpdateRolePermission(TblRolePermission rolePermission)
        {
            var rolePermissions = await _context.TblRolePermissions.FindAsync(rolePermission.Id);

            if(rolePermissions == null) return 0;

            _context.TblRolePermissions.Update(rolePermission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Role Assignment
        public async Task<int> UpdateUserRoleAssignment(TblRoleAssignment userRoleAssignment)
        {
            var roleAssignment = await _context.TblRoleAssignments.FindAsync(userRoleAssignment.Id);

            if (roleAssignment == null) return 0;

            _context.TblRoleAssignments.Update(userRoleAssignment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Role
        public async Task<int> DeactivateRole(int id)
        {
            var role = await _context.TblRoleMasters.FindAsync(id);

            if (role == null) return 0;

            role.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Role Permission
        public async Task<int> DeactivateRolePermission(int id)
        {
            var rolePermission = await _context.TblRolePermissions.FindAsync(id);

            if(rolePermission == null) return 0;

            rolePermission.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Role Assignment
        public async Task<int> DeactivateRoleAssignment(int id)
        {
            var roleAssignment = await _context.TblRoleAssignments.FindAsync(id);

            if (roleAssignment == null) return 0;

            roleAssignment.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Role By Id
        public async Task<TblRoleMaster> GetRoleById(int id)
        {
            var role = await _context.TblRoleMasters.FindAsync(id);

            return role;
        }
        #endregion

        #region Get Role Permission By Id
        public async Task<TblRolePermission> GetRolePermissionById(int id)
        {
            var rolePermission = await _context.TblRolePermissions.Where(x => x.Id == id).Include(x => x.TblRoleMaster).FirstOrDefaultAsync();

            return rolePermission;
        }
        #endregion

        #region Get User Assign Role By Id
        public async Task<TblRoleAssignment> GetUserAssignRoleById(int id)
        {
            var userAssignRole = await _context.TblRoleAssignments.Where(x => x.Id == id).Include(x => x.TblRoleMaster)
                                                                  .Include(x => x.TblUserMaster).FirstOrDefaultAsync(); ;

            return userAssignRole;
        }
        #endregion

        #region Get All Role
        public async Task<Response<TblRoleMaster>> GetRoles(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblRoleMasters.Count() / pageResult);

            var roles = await _context.TblRoleMasters.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();

            var rolesResponse = new Response<TblRoleMaster>()
            {
                Values = roles,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return rolesResponse;
        }
        #endregion

        #region Get All Role Permission
        public async Task<Response<TblRolePermission>> GetRolePermissions(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblRolePermissions.Count() / pageResult);

            List<TblRolePermission> rolePermissions = await _context.TblRolePermissions.Skip((page - 1) * (int)pageResult)
                                                                  .Take((int)pageResult).Include(r => r.TblRoleMaster).ToListAsync();

            var rolesResponse = new Response<TblRolePermission>()
            {
                Values = rolePermissions,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return rolesResponse;
        }
        #endregion

        #region Get All User Assign role
        public async Task<Response<TblRoleAssignment>> GetUserAssignRoles(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblRoleAssignments.Count() / pageResult);

            List<TblRoleAssignment> userAssignRoles = await _context.TblRoleAssignments.Skip((page - 1) * (int)pageResult)
                                                                .Take((int)pageResult).Include(r => r.TblRoleMaster)
                                                                .Include(u => u.TblUserMaster).ToListAsync();

            var userAssignRolesResponse = new Response<TblRoleAssignment>()
            {
                Values = userAssignRoles,
                Pagination = new Pagination
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return userAssignRolesResponse;
        }
        #endregion
    }
}
