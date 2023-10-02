using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutualfundService
    {
        Task<MFTransactionDto<MutualFundDto>> GetClientwiseMutualFundTransactionAsync(int userId, string? schemeName, string? folioNo
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate);
        Task<MFTransactionDto<MFSummaryDto>> GetMFSummaryAsync(int userId, bool? isBalanceUnitZero, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<MFCategoryWiseDto>> GetMFCategoryWiseAsync(int userId, bool? isBalanceUnitZero, string? searchingParams, SortingParams sortingParams);
        Task<MFTransactionDto<AllClientMFSummaryDto>> GetAllClientMFSummaryAsync(bool? isBalanceUnitZero, DateTime fromDate, DateTime toDate, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<UserNameDto>> GetMFUserNameAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SchemaNameDto>> DisplayschemeNameAsync(int userId, string? folioNo, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<SchemaNameDto>> DisplayFolioNoAsync(int userId, string? schemeName, string? searchingParams, SortingParams sortingParams);
        Task<int> ImportNJClientFileAsync(IFormFile file, bool updateIfExist);
        Task<int> ImportCAMSFileAsync(IFormFile file, string password, bool UpdateIfExist);
        Task<int> ImportNJDailyPriceFileAsync(IFormFile formFile);
        Task<(int, string)> ImportAMFINAVFileAsync();
    }
}
