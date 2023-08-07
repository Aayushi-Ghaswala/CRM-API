using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

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
            if (_context.TblRoleMasters.Any(x => x.RoleName.ToLower() == roleMaster.RoleName.ToLower() && !x.IsDeleted))
                return 0;

            await _context.TblRoleMasters.AddAsync(roleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add RolePermission
        public async Task<int> AddRolePermission(TblRolePermission rolePermission)
        {
            if (_context.TblRolePermissions.Any(x => x.RoleId == rolePermission.RoleId && x.ModuleId == rolePermission.ModuleId && !x.IsDeleted))
                return 0;
            await _context.TblRolePermissions.AddAsync(rolePermission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add UserRoleAssignment
        public async Task<int> AddUserRoleAssignment(TblRoleAssignment userRoleAssignment)
        {
            if (_context.TblRoleAssignments.Any(x => x.UserId == userRoleAssignment.UserId && x.RoleId == userRoleAssignment.RoleId && !x.IsDeleted))
                return 0;
            await _context.TblRoleAssignments.AddAsync(userRoleAssignment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Add Module
        public async Task<int> AddModule(TblModuleMaster moduleMaster)
        {
            if (_context.TblModuleMasters.Any(x => x.ModuleName.ToLower() == moduleMaster.ModuleName.ToLower() && !x.IsDeleted))
                return 0;

            await _context.TblModuleMasters.AddAsync(moduleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role
        public async Task<int> UpdateRole(TblRoleMaster roleMaster)
        {
            var role = _context.TblRoleMasters.AsNoTracking().Where(x => x.RoleId == roleMaster.RoleId);

            if (role == null) return 0;

            _context.TblRoleMasters.Update(roleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Role Permission
        public async Task<int> UpdateRolePermission(TblRolePermission rolePermission)
        {
            var rolePermissions = _context.TblRolePermissions.AsNoTracking().Where(x => x.Id == rolePermission.Id);

            if (rolePermissions == null) return 0;

            _context.TblRolePermissions.Update(rolePermission);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update User Role Assignment
        public async Task<int> UpdateUserRoleAssignment(TblRoleAssignment userRoleAssignment)
        {
            var roleAssignment = _context.TblRoleAssignments.AsNoTracking().Where(x => x.Id == userRoleAssignment.Id);

            if (roleAssignment == null) return 0;

            _context.TblRoleAssignments.Update(userRoleAssignment);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Module
        public async Task<int> UpdateModule(TblModuleMaster moduleMaster)
        {
            var module = _context.TblModuleMasters.AsNoTracking().Where(x => x.Id == moduleMaster.Id);

            if (module == null) return 0;

            _context.TblModuleMasters.Update(moduleMaster);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Role
        public async Task<int> DeactivateRole(int id)
        {
            var role = await _context.TblRoleMasters.FindAsync(id);
            if (_context.TblRoleAssignments.Any(x => x.RoleId == id && !x.IsDeleted))
                return 0;

            role.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate Role Permission
        public async Task<int> DeactivateRolePermission(int id)
        {
            var rolePermission = await _context.TblRolePermissions.FindAsync(id);

            if (_context.TblRoleAssignments.Any(x => x.RoleId == rolePermission.RoleId && !x.IsDeleted)) return 0;

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

        #region Deactivate Module
        public async Task<int> DeactivateModule(int id)
        {
            var module = await _context.TblModuleMasters.FindAsync(id);
            if (_context.TblRolePermissions.Any(x => x.ModuleId == id && !x.IsDeleted))
                return 0;

            module.IsDeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Role By Id
        public async Task<TblRoleMaster> GetRoleById(int id)
        {
            var role = await _context.TblRoleMasters.Where(x => x.IsDeleted != true && x.RoleId == id).FirstOrDefaultAsync();

            return role;
        }
        #endregion

        #region Get Role Permission By Id
        public async Task<TblRolePermission> GetRolePermissionById(int id)
        {
            var rolePermission = await _context.TblRolePermissions.Where(x => x.Id == id && x.IsDeleted != true).Include(x => x.TblRoleMaster).FirstOrDefaultAsync();

            return rolePermission;
        }
        #endregion

        #region Get User Assign Role By Id
        public async Task<TblRoleAssignment> GetUserAssignRoleById(int id)
        {
            var userAssignRole = await _context.TblRoleAssignments.Where(x => x.Id == id && x.IsDeleted != true).Include(x => x.TblRoleMaster)
                                                                  .Include(x => x.TblUserMaster).FirstOrDefaultAsync(); ;

            return userAssignRole;
        }
        #endregion

        #region Get All Role
        public async Task<Response<TblRoleMaster>> GetRoles(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblRoleMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblRoleMaster>(search).Where(x => x.IsDeleted != true).AsQueryable();
            }
            else
            {
                filterData = _context.TblRoleMasters.Where(x => x.IsDeleted != true).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var rolesResponse = new Response<TblRoleMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return rolesResponse;
        }
        #endregion

        #region Get All Role Permission
        public async Task<Response<TblRolePermission>> GetRolePermissions(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblRolePermission>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblRolePermission>(search).Where(x => x.IsDeleted != true).Include(r => r.TblRoleMaster).Include(x => x.TblModuleMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblRolePermissions.Where(x => x.IsDeleted != true).Include(r => r.TblRoleMaster).Include(x => x.TblModuleMaster).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var rolePermissionsResponse = new Response<TblRolePermission>()
            {
                Values = paginatedData,
                Pagination = new Pagination
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return rolePermissionsResponse;
        }
        #endregion

        #region Get All User Assign role
        public async Task<Response<TblRoleAssignment>> GetUserAssignRoles(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblRoleAssignment>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblRoleAssignment>(search).Where(x => x.IsDeleted != true).Include(r => r.TblRoleMaster)
                                                                    .Include(u => u.TblUserMaster).AsQueryable();
            }
            else
            {
                filterData = _context.TblRoleAssignments.Where(x => x.IsDeleted != true).Include(r => r.TblRoleMaster)
                                                                    .Include(u => u.TblUserMaster).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var userAssignRolesResponse = new Response<TblRoleAssignment>()
            {
                Values = paginatedData,
                Pagination = new Pagination
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return userAssignRolesResponse;
        }
        #endregion

        #region Get All Module
        public async Task<Response<TblModuleMaster>> GetModules(string search, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = new List<TblModuleMaster>().AsQueryable();

            if (search != null)
            {
                filterData = _context.Search<TblModuleMaster>(search).Where(x => !x.IsDeleted).AsQueryable();
            }
            else
            {
                filterData = _context.TblModuleMasters.Where(x => !x.IsDeleted).AsQueryable();
            }

            pageCount = Math.Ceiling((filterData.Count() / sortingParams.PageSize));

            // Apply sorting
            var sortedData = SortingExtensions.ApplySorting(filterData, sortingParams.SortBy, sortingParams.IsSortAscending);

            // Apply pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var modulesResponse = new Response<TblModuleMaster>()
            {
                Values = paginatedData,
                Pagination = new Pagination
                {
                    CurrentPage = sortingParams.PageNumber,
                    Count = (int)pageCount
                }
            };

            return modulesResponse;
        }
        #endregion
    }
}
