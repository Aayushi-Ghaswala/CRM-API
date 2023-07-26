using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Account_Module
{
    public interface IAccountTransactionservice
    {
        Task<string> GetTransactionDocNoAsync(string? filterString, string? docNo = null);
        Task<ResponseDto<AccountTransactionDto>> GetAccountTransactionAsync(string filterString, string? searchingParams, SortingParams sortingParams);
        Task<int> AddAccountTransactionAsync(AddAccountTransactionDto addAccountTransaction);
        Task<int> UpdateAccountTransactionAsync(UpdateAccountTransactionDto updateAccountTransaction);
    }
}
