using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.RequestModel.Sales_Module;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ILeadService
    {
        Task<ResponseDto<LeadDto>> GetLeadsAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<InvestmentTypeDto>> GetInvestmentTypesAsync(string search, SortingParams sortingParams);
        Task<LeadDto> GetLeadByIdAsync(int id);
        Task<LeadDto> GetLeadByNameAsync(string Name);
        Task<int> AddLeadAsync(AddLeadDto leadDto);
        Task<int> UpdateLeadAsync(UpdateLeadDto leadDto);
        Task<int> DeactivateLeadAsync(int id);
        int SendLeadEmailAsync(LeadDto leadDto, string userName);
        int SendLeadSMSAsync(LeadDto leadDto, string userName);
    }
}
