﻿using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;

namespace CRM_api.Services.IServices.Sales_Module
{
    public interface ILeadService
    {
        Task<ResponseDto<LeadDto>> GetLeadsAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<InvesmentTypeDto>> GetInvestmentTypesAsync(string search, SortingParams sortingParams);
        Task<LeadDto> GetLeadByIdAsync(int id);
        Task<LeadDto> GetLeadByNameAsync(string Name);
        Task<ResponseDto<LeadDto>> GetLeadByAssigneeAsync(int assignedTo, string search, SortingParams sortingParams);
        Task<ResponseDto<LeadDto>> GetLeadByNoAssigneeAsync(string search, SortingParams sortingParams);
        Task<byte[]> GetLeadsForCSVAsync(string search, SortingParams sortingParams);
        int CheckMobileExistAsync(int? id, string mobileNo);
        Task<int> AddLeadAsync(AddLeadDto leadDto);
        Task<int> UpdateLeadAsync(UpdateLeadDto leadDto);
        Task<int> DeactivateLeadAsync(int id);
        int SendLeadEmailAsync(LeadDto leadDto);
        int SendLeadSMSAsync(LeadDto leadDto);
    }
}
