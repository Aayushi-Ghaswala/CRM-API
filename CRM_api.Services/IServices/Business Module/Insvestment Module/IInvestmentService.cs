using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Insvestment_Module
{
    public interface IInvestmentService
    {
        Task<ResponseDto<InvestmentTypeDto>> GetInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive);
        Task<InvestmentTypeDto> GetInvestmentTypeByIdAsync(int id);
        Task<ResponseDto<SubInvestmentTypeDto>> GetSubInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive);
        Task<ResponseDto<SubInvestmentTypeDto>> GetSubInvestmentTypeByInvIdAsync(int invId, string? search, SortingParams sortingParams);
        Task<ResponseDto<SubsubInvTypeDto>> GetSubsubInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive);
        Task<ResponseDto<SubsubInvTypeDto>> GetSubsubInvestmentTypeBySubInvIdAsync(int subInvId, string? search, SortingParams sortingParams);
        Task<int> AddInvestmentTypeAsync(AddInvestmentTypeDto addInvestmentTypeDto);
        Task<int> AddSubInvestmentTypeAsync(AddSubInvestmentTypeDto addSubInvestmentTypeDto);
        Task<int> AddSubsubInvestmentTypeAsync(AddSubsubInvTypeDto addSubsubInvTypeDto);
        Task<int> UpdateInvestmentTypeAsync(UpdateInvestmentTypeDto updateInvestmentTypeDto);
        Task<int> UpdateSubInvestmentTypeAsync(UpdateSubInvestmentTypeDto updateSubInvestmentTypeDto);
        Task<int> UpdateSubsubInvestmentTypeAsync(UpdateSubsubInvTypeDto updateSubsubInvTypeDto);
        Task<int> DeactiveInvestmentTypeAsync(int id);
        Task<int> DeactiveSubInvestmentTypeAsync(int id);
        Task<int> DeactiveSubsubInvestmentTypeAsync(int id);
    }
}
