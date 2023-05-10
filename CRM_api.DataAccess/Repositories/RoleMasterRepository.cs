using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories;
using CRM_api.DataAccess.Model;
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
        public async Task<int> AddRole(RoleMaster roleMaster)
        {
            await _context.RoleMasters.AddAsync(roleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add RolePermission
        public async Task<int> AddRolePermission(RolePermission rolePermission)
        {
            await _context.RolePermissions.AddAsync(rolePermission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add UserRoleAssignment
        public async Task<int> AddUserRoleAssignment(UserRoleAssignment userRoleAssignment)
        {
            await _context.UserRoleAssignments.AddAsync(userRoleAssignment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role
        public async Task<int> UpdateRole(RoleMaster roleMaster)
        {
            _context.RoleMasters.Update(roleMaster);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role Permission
        public async Task<int> UpdateRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Update(rolePermission);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Role Assignment
        public async Task<int> UpdateUserRoleAssignment(UserRoleAssignment userRoleAssignment)
        {
            _context.UserRoleAssignments.Update(userRoleAssignment);

            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Role By Id
        public async Task<RoleMaster> GetRoleById(int id)
        {
            var role = await _context.RoleMasters.FindAsync(id);
            ArgumentNullException.ThrowIfNull(role, "No Role Found...");

            return role;
        }
        #endregion

        #region Get Role Permission By Id
        public async Task<RolePermission> GetRolePermissionById(int id)
        {
            var rolePermission = await _context.RolePermissions.FindAsync(id);
            ArgumentNullException.ThrowIfNull(rolePermission, "No Role Permission Found...");

            return rolePermission;
        }
        #endregion

        #region Get User Assign Role By Id
        public async Task<UserRoleAssignment> GetUserAssignRoleById(int id)
        {
            var userAssignRole = await _context.UserRoleAssignments.FindAsync(id);
            ArgumentNullException.ThrowIfNull(userAssignRole, "No User Assign Role Found...");

            return userAssignRole;
        }
        #endregion

        #region Get All Role
        public async Task<IEnumerable<RoleMaster>> GetRoles()
        {
            var roles = await _context.RoleMasters.ToListAsync();
            if (roles.Count == 0)
                throw new Exception("Role Not Found...");

            return roles;
        }
        #endregion

        #region Get All Role Permission
        public async Task<IEnumerable<RolePermission>> GetRolePermissions()
        {
            List<RolePermission> rolePermissions = await _context.RolePermissions.Include(r => r.RoleMaster).ToListAsync();
            if (rolePermissions.Count == 0)
                throw new Exception("Role Permission Not Found");

            return rolePermissions;
        }
        #endregion

        #region Get All User Assign role
        public async Task<IEnumerable<UserRoleAssignment>> GetUserAssignRoles()
        {
            List<UserRoleAssignment> userAssignRoles = await _context.UserRoleAssignments.Include(r => r.RoleMaster).Include(u => u.UserMaster).ToListAsync();
            if (userAssignRoles.Count == 0)
                throw new Exception("User Role Assignment Not Found");

            return userAssignRoles;
        }
        #endregion
    }
}
