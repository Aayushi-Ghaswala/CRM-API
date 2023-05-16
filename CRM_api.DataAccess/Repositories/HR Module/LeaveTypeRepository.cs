using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.HR_Module
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly CRMDbContext _context;

        public LeaveTypeRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Get LeaveType by Id
        public async Task<TblLeaveType> GetLeaveTypeById(int id)
        {
            var leaveType = await _context.TblLeaveTypes.FirstAsync(x => x.LeaveId == id);
            return leaveType;
        }
        #endregion

        #region Get LeaveType by Name
        public async Task<TblLeaveType> GetLeaveTypeByName(string Name)
        {
            var leaveType = await _context.TblLeaveTypes.FirstAsync(x => x.Name.ToLower() == Name.ToLower());
            return leaveType;
        }
        #endregion

        #region Get LeaveTypes
        public async Task<Response<TblLeaveType>> GetLeaveTypes(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblLeaveTypes.Where(x => x.Isdeleted == false).Count() / pageResult);

            var leaveTypes = await _context.TblLeaveTypes.Where(x => x.Isdeleted == false).Skip((page - 1) * (int)pageResult)
                                                     .Take((int)pageResult).ToListAsync();

            var leaveTypeResponse = new Response<TblLeaveType>()
            {
                Values = leaveTypes,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return leaveTypeResponse;
        }
        #endregion

        #region Add LeaveType
        public async Task<int> AddLeaveType(TblLeaveType leaveType)
        {
            if (_context.TblLeaveTypes.Any(x => x.Name == leaveType.Name))
                return 0;

            _context.TblLeaveTypes.Add(leaveType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update LeaveType
        public async Task<int> UpdateLeaveType(TblLeaveType leaveType)
        {
            _context.TblLeaveTypes.Update(leaveType);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Deactivate LeaveType
        public async Task<int> DeactivateLeaveType(int id)
        {
            var leaveType = await _context.TblLeaveTypes.FindAsync(id);

            if(leaveType == null) return 0;

            leaveType.Isdeleted = true;
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}