using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MutualFunds_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class MutualfundProfile : Profile
    {
        public MutualfundProfile()
        {
            CreateMap<AddMutualfundsDto, TblMftransaction>()
                .AfterMap((dto, Mutualfund) =>
                {
                    Mutualfund.Notes = null;
                });
            CreateMap<AddMutualfundsDto, TblNotexistuserMftransaction>();
            CreateMap<TblMftransaction, MutualFundDto>()
                .ForMember(x => x.trnid, opt => opt.MapFrom(dest => dest.Trnid));
            CreateMap<Response<TblMftransaction>, ResponseDto<MutualFundDto>>();
            CreateMap<TblMftransaction, SchemaNameDto>();
            CreateMap<Response<TblMftransaction>, ResponseDto<SchemaNameDto>>();
            CreateMap<BussinessResponse<TblMftransaction>, MFTransactionDto<MutualFundDto>>();
            CreateMap<MutualFundSummary, MFSummaryDto>();
            CreateMap<TblMftransaction, MFSummaryDto>()
                .ForMember(x => x.TotalPurchaseUnit, opt => opt.MapFrom(dest => dest.Noofunit))
                .ForMember(x => x.TotalRedemptionUnit, opt => opt.MapFrom(dest => dest.Noofunit))
                .ForMember(x => x.BalanceUnit, opt => opt.MapFrom(dest => dest.Noofunit))
                .ForMember(x => x.CurrentValue, opt => opt.MapFrom(dest => dest.Invamount));
            CreateMap<TblMftransaction, MFUserNameDto>();
            CreateMap<Response<TblMftransaction>, ResponseDto<MFUserNameDto>>();
        }
    }
}
