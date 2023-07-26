using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Account_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class AccountTransactionProfile : Profile
    {
        public AccountTransactionProfile()
        {
            CreateMap<AddAccountTransactionDto, TblAccountTransaction>();
            CreateMap<UpdateAccountTransactionDto, TblAccountTransaction>();
            CreateMap<TblAccountTransaction, AccountTransactionDto>();
            CreateMap<Response<TblAccountTransaction>, ResponseDto<AccountTransactionDto>>();
        }
    }
}
