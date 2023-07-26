using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<TblAccountMaster, AccountMasterDto>();
            CreateMap<Response<TblAccountMaster>, ResponseDto<AccountMasterDto>>();
            CreateMap<TblAccountGroupMaster, AccountGroupDto>();
            CreateMap<Response<TblAccountGroupMaster>, ResponseDto<AccountGroupDto>>();
            CreateMap<TblCompanyMaster, CompanyMasterDto>();
            CreateMap<Response<TblCompanyMaster>, ResponseDto<CompanyMasterDto>>();
            CreateMap<TblFinancialYearMaster, FinancialYearDto>();
            CreateMap<Response<TblFinancialYearMaster>, ResponseDto<FinancialYearDto>>();
            CreateMap<TblAccountOpeningBalance, AccountOpeningBalanceDto>();
            CreateMap<Response<TblAccountOpeningBalance>, ResponseDto<AccountOpeningBalanceDto>>();
            CreateMap<AddUserAccountDto, TblAccountMaster>();
            CreateMap<AddAccountGroupDto, TblAccountGroupMaster>();
            CreateMap<AddCompanyDto, TblCompanyMaster>();
            CreateMap<AddFinancialYearDto, TblFinancialYearMaster>();
            CreateMap<AddAccountOpeningBalanceDto, TblAccountOpeningBalance>();
            CreateMap<UpdateUserAccountDto, TblAccountMaster>();
            CreateMap<UpdateAccountGroupDto, TblAccountGroupMaster>();
            CreateMap<UpdateCompanyDto, TblCompanyMaster>();
            CreateMap<UpdateFinancialYearDto, TblFinancialYearMaster>();
            CreateMap<UpdateAccountOpeningBalanceDto, TblAccountOpeningBalance>();
        }
    }
}
