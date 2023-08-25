using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.IServices.Business_Module.WBC_Module
{
    public interface IWBCService
    {
        Task<List<GoldPointCategoryDto>> GetPointCategoryAsync();
        Task<ResponseDto<UserNameDto>> GetGPUsernameAsync(string? type, string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<WbcTypeDto>> GetGPTypesAsync(int? userId, string? searchingParams, SortingParams sortingParams);
        Task<GoldPointResponseDto<GoldPointDto>> GetGPLedgerReportAsync(DateTime? date, int? userId, string? type, int? categoryId, string? searchingParams, SortingParams sortingParams);
        Task<List<WbcGPResponseDto>> GetGPAsync(DateTime date);
        Task<(int,string)> ReleaseGPAsync(DateTime date, bool isTruncate);
        Task<ResponseDto<WbcTypeDto>> GetAllWbcSchemeTypesAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<WBCSchemeMasterDto>> GetAllWbcSchemesAsync(string? searchingParams, SortingParams sortingParams);
        Task<ResponseDto<ReferenceTrackingResponseDto>> GetDirectReferralsAsync(int userId, string? search, SortingParams sortingParams);
        Task<List<ReferenceTrackingResponseDto>> GetReferredByListAsync(int? userId);
        Task<int> AddWbcSchemeAsync(AddWBCSchemeDto addWBCSchemeDto);
        Task<int> UpdateWbcSchemeAsync(UpdateWBCSchemeDto updateWBCSchemeDto);
        Task<int> DeleteWbcSchemeAsync(int id);
    }
}
