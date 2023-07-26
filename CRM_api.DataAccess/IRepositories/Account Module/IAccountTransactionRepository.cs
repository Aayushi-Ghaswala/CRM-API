using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Account_Module
{
    public interface IAccountTransactionRepository
    {
        Task<TblAccountTransaction> GetLastAccountTrasaction(string? filterString, string? number);
        Task<TblInvesmentType> GetInvestmentType(string? name);
        Task<Response<TblAccountTransaction>> GetAccountTransaction(string filterString, string? searchingParams, SortingParams sortingParams);
        Task<int> AddAccountTransaction(TblAccountTransaction tblAccountTransaction);
        Task<int> UpdateAccountTransaction(TblAccountTransaction tblAccountTransaction);
    }
}
