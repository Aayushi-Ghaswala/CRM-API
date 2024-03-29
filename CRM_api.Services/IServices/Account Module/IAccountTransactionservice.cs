﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Account_Module
{
    public interface IAccountTransactionservice
    {
        Task<string> GetTransactionDocNoAsync(string? filterString, string? docNo = null);
        Task<ResponseDto<PaymentTypeDto>> GetPaymentTypesAsync(string? search, SortingParams sortingParams);
        Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal?>)> GetAccountTransactionAsync(int? companyId, int? financialYearId, string filterString, string? searchingParams, SortingParams sortingParams);
        Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal?>)> GetCompanyAndAccountWiseTransactionAsync(int? companyId, int? accountId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams, bool isBankBook = false);
        Task<(List<TrialBalanceDto>, Dictionary<string, decimal>)> CalculateTrailBalanceByCompanyIdAsync(int companyId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams);
        Task<(ResponseDto<AccountTransactionDto>, Dictionary<string, decimal>)> GetCompanyWiseJVTransactionAsync(int companyId, DateTime startDate, DateTime endDate, string? search, SortingParams sortingParams);
        Task<int> AddAccountTransactionAsync(AddAccountTransactionDto addAccountTransaction);
        Task<int> UpdateAccountTransactionAsync(UpdateAccountTransactionDto updateAccountTransaction);
        Task<int> DeleteAccountTransactionAsync(string? docNo, string docType, int companyId);
    }
}
