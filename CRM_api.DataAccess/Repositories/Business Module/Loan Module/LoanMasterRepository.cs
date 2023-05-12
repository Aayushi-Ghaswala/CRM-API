using CRM_api.DataAccess.Context;
using CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.User_Module;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Repositories.Business_Module.Loan_Module
{
    public class LoanMasterRepository : ILoanMasterRepository
    {
        private readonly CRMDbContext _context;

        public LoanMasterRepository(CRMDbContext context)
        {
            _context = context;
        }

        #region Add Loan Detail
        public async Task<int> AddLoanDetail(TblLoanMaster tblLoan)
        {
            _context.TblLoanMasters.Add(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Update Loan Detail
        public async Task<int> UpdateLoanDetail(TblLoanMaster tblLoan)
        {
            _context.TblLoanMasters.Update(tblLoan);
            return await _context.SaveChangesAsync();
        }
        #endregion

        #region Get Loan Detail By Id
        public async Task<TblLoanMaster> GetLoanDetailById(int id)
        {
            var loanDetail = await _context.TblLoanMasters.Where(x => x.Id == id).Include(u => u.TblUserMaster).Include(c => c.TblUserCategoryMaster)
                                                            .FirstOrDefaultAsync();

            return loanDetail;
        }
        #endregion

        #region Get All Loan Details
        public async Task<Response<TblLoanMaster>> GetLoanDetails(int page)
        {
            float pageResult = 10f;
            var pageCount = Math.Ceiling(_context.TblLoanMasters.Count() / pageResult);

            var loanDetails = await _context.TblLoanMasters.Skip((page - 1) * (int)pageResult).Take((int)pageResult).Include(u => u.TblUserMaster)
                                                           .Include(c => c.TblUserCategoryMaster).ToListAsync();

            var loanResponse = new Response<TblLoanMaster>()
            {
                Values = loanDetails,
                Pagination = new Pagination()
                {
                    CurrentPage = page,
                    Count = (int)pageCount
                }
            };

            return loanResponse;
        }
        #endregion

    }
}
