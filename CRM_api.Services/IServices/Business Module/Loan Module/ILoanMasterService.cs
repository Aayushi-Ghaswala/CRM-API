﻿using CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.Business_Module.Loan_Module
{
    public interface ILoanMasterService
    {
        Task<int> AddLoanDetailAsync(AddLoanMasterDto loanMasterDto);
        Task<int> UpdateLoanDetailAsync(UpdateLoanMasterDto loanMasterDto);
        Task<ResponseDto<LoanMasterDto>> GetLoanDetailsAsync(int page);
        Task<LoanMasterDto> GetLoanDetailByIdAsync(int id);
    }
}