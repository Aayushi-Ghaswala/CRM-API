using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutualfundService
    {
        Task<MFTransactionDto<MutualFundDto>> GetClientwiseMutualFundTransaction(int userId, int? schemeId
            , string? searchingParams, SortingParams sortingParams, DateTime? StartDate, DateTime? EndDate);
        Task<MFTransactionDto<MFSummaryDto>> GetMFSummary(int userId, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<MFCategoryWiseDto>> GetMFCategoryWise(int userId, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<AllClientMFSummaryDto>> GetAllClientMFSummary(DateTime FromDate, DateTime ToDate, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<MFUserNameDto>> GetMFUserName(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SchemaNameDto>> DisplayschemeName(int userId, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportNJClientFile(IFormFile file, bool UpdateIfExist);
    }
}
