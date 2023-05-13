using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module
{
    public interface ILoanMasterRepository
    {
        Task<int> AddLoanDetail(TblLoanMaster tblLoan);
        Task<int> UpdateLoanDetail(TblLoanMaster tblLoan);
        Task<Response<TblLoanMaster>> GetLoanDetails(int page);
        Task<TblLoanMaster> GetLoanDetailById(int id);
    }
}
