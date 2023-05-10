using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories
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
            _context.TblRoleMasters.Update(roleMaster);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role Permission
        public async Task<int> UpdateRolePermission(TblRolePermission rolePermission)
        {
            _context.TblRolePermissions.Update(rolePermission);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Role Assignment
        public async Task<int> UpdateUserRoleAssignment(TblRoleAssignment userRoleAssignment)
        {
            _context.TblRoleAssignments.Update(userRoleAssignment);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Role By Id
        public async Task<TblRoleMaster> GetRoleById(int id)
        {
            var role = await _context.TblRoleMasters.FindAsync(id);
            ArgumentNullException.ThrowIfNull(role, "No Role Found...");

            return role;
        }
        #endregion

        #region Get Role Permission By Id
        public async Task<TblRolePermission> GetRolePermissionById(int id)
        {
            var rolePermission = await _context.TblRolePermissions.FindAsync(id);
            ArgumentNullException.ThrowIfNull(rolePermission, "No Role Permission Found...");

            return rolePermission;
        }
        #endregion

        #region Get User Assign Role By Id
        public async Task<TblRoleAssignment> GetUserAssignRoleById(int id)
        {
            var userAssignRole = await _context.TblRoleAssignments.FindAsync(id);
            ArgumentNullException.ThrowIfNull(userAssignRole, "No User Assign Role Found...");

            return userAssignRole;
        }
        #endregion

        #region Get All Role
        public async Task<IEnumerable<TblRoleMaster>> GetRoles()
        {
            var roles = await _context.TblRoleMasters.ToListAsync();
            if (roles.Count == 0)
                throw new Exception("Role Not Found...");

            return roles;
        }
        #endregion

        #region Get All Role Permission
        public async Task<IEnumerable<TblRolePermission>> GetRolePermissions()
        {
            List<TblRolePermission> rolePermissions = await _context.TblRolePermissions.Include(r => r.TblRoleMaster).ToListAsync();
            if (rolePermissions.Count == 0)
                throw new Exception("Role Permission Not Found");

            return rolePermissions;
        }
        #endregion

        #region Get All User Assign role
        public async Task<IEnumerable<TblRoleAssignment>> GetUserAssignRoles()
        {
            List<TblRoleAssignment> userAssignRoles = await _context.TblRoleAssignments.Include(r => r.TblRoleMaster).Include(u => u.TblUserMaster).ToListAsync();
            if (userAssignRoles.Count == 0)
                throw new Exception("User Role Assignment Not Found");

            return userAssignRoles;
        }
        #endregion
    }
}
