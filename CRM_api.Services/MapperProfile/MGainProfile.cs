using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class MGainProfile : Profile
    {
        public MGainProfile()
        {
            CreateMap<TblMgaindetail, MGainDetailsDto>();
            CreateMap<Response<TblMgaindetail>, ResponseDto<MGainDetailsDto>>();
            CreateMap<MGainBussinessResponse<TblMgaindetail>, MGainResponseDto<MGainDetailsDto>>();
            CreateMap<TblMgainPaymentMethod, MGainPaymentDto>();
            CreateMap<TblMgainCurrancyMaster, MGainCurrancyDto>();
            CreateMap<TblProjectMaster, ProjectMasterDto>();
            CreateMap<Response<TblProjectMaster>, ResponseDto<ProjectMasterDto>>();
            CreateMap<TblPlotMaster, PlotMasterDto>();
            CreateMap<UpdateMGainPaymentDto, TblMgainPaymentMethod>()
                .ForMember(x => x.CurrancyId, opt => opt.MapFrom(src => src.CurrencyId));
            CreateMap<TblMgaindetail, MGainPaymentRecieptDto>();
            CreateMap<AddMGainDetailsDto, TblMgaindetail>()
                .AfterMap((dto, entity) =>
                {
                    entity.MgainModeholder = "Single";
                    if (dto.MgainIsSecondHolder == true)
                        entity.MgainModeholder = "Joint";
                    else
                    {
                        entity.Mgain2ndholdername = null;
                        entity.Mgain2ndholderDob = null;
                        entity.Mgain2ndholderGender = null;
                        entity.Mgain2ndholderMaritalStatus = null;
                        entity.Mgain2ndholderFatherName = null;
                        entity.Mgain2ndholderAddress = null;
                        entity.Mgain2ndholderPan = null;
                        entity.Mgain2ndholderAadhar = null;
                        entity.Mgain2ndholderMobile = null;
                        entity.Mgain2ndholderOccupation = null;
                        entity.Mgain2ndholderStatus = null;
                        entity.Mgain2ndholderSignature = null;
                        entity.Mgain2ndholderEmail = null;
                    }

                    entity.MgainAccountnum = 0;
                    entity.MgainAggre = "Pending";
                    entity.MgainIsactive = true;
                });

            CreateMap<AddMGainPaymentDto, TblMgainPaymentMethod>()
                .ForMember(x => x.CurrancyId, opt => opt.MapFrom(src => src.CurrencyId));
            CreateMap<UpdateMGainDetailsDto, TblMgaindetail>()
                .AfterMap((dto, entity) =>
                {
                    entity.MgainModeholder = "Single";
                    if (dto.MgainIsSecondHolder == true)
                        entity.MgainModeholder = "Joint";
                    else
                    {
                        entity.Mgain2ndholdername = null;
                        entity.Mgain2ndholderDob = null;
                        entity.Mgain2ndholderGender = null;
                        entity.Mgain2ndholderMaritalStatus = null;
                        entity.Mgain2ndholderFatherName = null;
                        entity.Mgain2ndholderAddress = null;
                        entity.Mgain2ndholderPan = null;
                        entity.Mgain2ndholderAadhar = null;
                        entity.Mgain2ndholderMobile = null;
                        entity.Mgain2ndholderOccupation = null;
                        entity.Mgain2ndholderStatus = null;
                        entity.Mgain2ndholderSignature = null;
                        entity.Mgain2ndholderEmail = null;
                    }

                    entity.MgainAccountnum = 0;
                    entity.MgainIsactive = true;
                    if (dto.MgainIsclosed is true)
                        entity.MgainIsactive = false;
                });
        }
    }
}
