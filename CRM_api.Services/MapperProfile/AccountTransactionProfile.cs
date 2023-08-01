using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class AccountTransactionProfile : Profile
    {
        public AccountTransactionProfile()
        {
            CreateMap<AddAccountTransactionDto, TblAccountTransaction>();
            CreateMap<UpdateAccountTransactionDto, TblAccountTransaction>();
            CreateMap<TblAccountTransaction, AccountTransactionDto>()
                .ForMember(x => x.DebitAccount, opt => opt.MapFrom(src => src.TblAccountMaster))
                .ForMember(x => x.CreditAccount, opt => opt.MapFrom(src => src.TblAccountMaster));
            CreateMap<Response<TblAccountTransaction>, ResponseDto<AccountTransactionDto>>();
            CreateMap<TblCompanyMaster, CompanyMasterDto>();
            CreateMap<TblPaymentTypeMaster, PaymentTypeDto>();
            CreateMap<Response<TblPaymentTypeMaster>, ResponseDto<PaymentTypeDto>>();
            CreateMap<TblInvesmentType, InvestmentTypeDto>();
            CreateMap<Response<TblInvesmentType>, ResponseDto<InvestmentTypeDto>>();
            CreateMap<TblMgainCurrancyMaster, MGainCurrancyDto>();
            CreateMap<TblAccountMaster, AccountMasterDto>();
        }
    }
}
