using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Account_Module
{
    public interface IAccountTransactionRepository
    {
        Task<TblAccountTransaction> GetLastAccountTrasaction(string? filterString, string? number);
        Task<TblInvesmentType> GetInvestmentType(string? name);
        Task<Response<TblPaymentTypeMaster>> GetPaymentType(string? search, SortingParams sortingParams);
        Task<TblPaymentTypeMaster> GetPaymentTypebyName(string? name);
        Task<Response<TblAccountTransaction>> GetAccountTransaction(int? companyId, int? financialYearId, string filterString, string? searchingParams, SortingParams sortingParams);
        Task<List<TblAccountTransaction>> GetCompanyAndAccountWiseTransaction(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams);
        Task<TblAccountTransaction> GetAccountTransactionByDocNo(string docNo, decimal? debit, decimal? credit);
        Task<TblAccountTransaction> GetAccountTransactionById(int id);
        Task<int> AddAccountTransaction(List<TblAccountTransaction> tblAccountTransactions);
        Task<int> UpdateAccountTransaction(List<TblAccountTransaction> tblAccountTransactions);
    }
}
