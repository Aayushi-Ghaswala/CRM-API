using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutualfundService
    {
        Task<MFTransactionDto<MutualFundDto>> GetClientwiseMutualFundTransactionAsync(int userId, int? schemeId
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate);
        Task<MFTransactionDto<MFSummaryDto>> GetMFSummaryAsync(int userId, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<MFCategoryWiseDto>> GetMFCategoryWiseAsync(int userId, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<AllClientMFSummaryDto>> GetAllClientMFSummaryAsync(DateTime fromDate, DateTime toDate, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<MFUserNameDto>> GetMFUserNameAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SchemaNameDto>> DisplayschemeNameAsync(int userId, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportNJClientFileAsync(IFormFile file, bool updateIfExist);
    }
}
