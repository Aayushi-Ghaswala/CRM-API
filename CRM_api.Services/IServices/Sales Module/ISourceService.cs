using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ISourceService
    {
        Task<ResponseDto<SourceDto>> GetSourcesAsync(string search, SortingParams sortingParams);
        Task<SourceDto> GetSourceByIdAsync(int id);
        Task<SourceDto> GetSourceByNameAsync(string Name);
        Task<int> AddSourceAsync(AddSourceDto sourceDto);
        Task<int> UpdateSourceAsync(UpdateSourceDto sourceDto);
        Task<int> DeactivateSourceAsync(int id);
    }
}
