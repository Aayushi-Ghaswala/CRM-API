using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Loan_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class LoanMasterProfile : Profile
    {
        public LoanMasterProfile()
        {
            CreateMap<AddLoanMasterDto, TblLoanMaster>()
                .AfterMap((dto, model) =>
                {
                    model.Frequency = "Monthly";
                });
            CreateMap<UpdateLoanMasterDto, TblLoanMaster>();

            CreateMap<TblUserMaster, UserNameDto>();
            CreateMap<TblLoanMaster, LoanMasterDto>().ReverseMap();
            CreateMap<Response<TblLoanMaster>, ResponseDto<LoanMasterDto>>();

            CreateMap<TblLoanTypeMaster, LoanTypeMasterDto>();
            CreateMap<TblBankMaster, BankMasterDto>();
            CreateMap<Response<TblBankMaster>, ResponseDto<BankMasterDto>>();
        }
    }
}
