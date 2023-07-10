using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ISourceTypeService
    {
        Task<ResponseDto<SourceTypeDto>> GetSourceTypesAsync(string search, SortingParams sortingParams);
        Task<SourceTypeDto> GetSourceTypeByIdAsync(int id);
        Task<SourceTypeDto> GetSourceTypeByNameAsync(string Name);
        Task<int> AddSourceTypeAsync(AddSourceTypeDto leaveTtypeDto);
        Task<int> UpdateSourceTypeAsync(UpdateSourceTypeDto leaveTypeDto);
        Task<int> DeactivateSourceTypeAsync(int id);
    }
}
