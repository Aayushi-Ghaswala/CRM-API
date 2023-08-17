using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;

namespace CRM_api.Services.IServices.WBC_Mall_Module
{
    public interface IMallProductService
    {
        Task<ResponseDto<MallProductDto>> GetMallProductsAsync(int? catId, string? filterString, string? search, SortingParams sortingParams);
        Task<int> AddMallProductAsync(AddMallProductDto addMallProductDto);
        Task<int> UpdateMallProductAsync(UpdateMallProductDto updateMallProductDto);
        Task<int> DeleteProductImageAsync(int id);
    }
}
