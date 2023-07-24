using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module
{
    public interface ILoanMasterRepository
    {
        Task<int> GetLoanDetailByUserId(int userId, DateTime date);
        Task<Response<TblLoanMaster>> GetLoanDetails(string? filterString, string search, SortingParams sortingParams);
        Task<Response<TblBankMaster>> GetBankDetails(SortingParams sortingParams);
        Task<TblLoanMaster> GetLoanDetailById(int id);
        List<TblLoanMaster> GetLoanDetailsForEMIReminder();
        Task<int> AddLoanDetail(TblLoanMaster tblLoan);
        Task<int> UpdateLoanDetail(TblLoanMaster tblLoan, bool flag = false);
        Task<int> DeactivateLoanDetail(int id);
    }
}
