using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public async Task<Response<TblRoleMaster>> GetRoles(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblRoleMasters.AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblRoleMaster>(searchingParams);
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
        public async Task<Response<TblRolePermission>> GetRolePermissions(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblRolePermissions.Include(r => r.TblRoleMaster).AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblRolePermission>(searchingParams);
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
        public async Task<Response<TblRoleAssignment>> GetUserAssignRoles(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            double pageCount = 0;

            var filterData = _context.TblRoleAssignments.AsQueryable();

            if (searchingParams.Count > 0)
            {
                filterData = _context.SearchByField<TblRoleAssignment>(searchingParams);
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
    }
}
