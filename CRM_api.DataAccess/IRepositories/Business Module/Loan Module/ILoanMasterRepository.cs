using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Loan_Module
{
    public interface ILoanMasterRepository
    {
        Task<Response<TblLoanMaster>> GetLoanDetails(Dictionary<string, object> searchingParams, SortingParams sortingParams);
        Task<TblLoanMaster> GetLoanDetailById(int id);
        Task<int> AddLoanDetail(TblLoanMaster tblLoan);
        Task<int> UpdateLoanDetail(TblLoanMaster tblLoan);
        Task<int> DeactivateLoanDetail(int id);
    }
}
