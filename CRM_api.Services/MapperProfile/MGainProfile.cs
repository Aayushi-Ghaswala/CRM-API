using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.MGain_Module;
using CRM_api.DataAccess.ResponseModel.Bussiness_Module.RealEstateModule;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;
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

            CreateMap<AddMGainPlotDetailsDto, TblMgainPlotData>();
            CreateMap<TblMgainPlotData, MGainPlotDto>();
            CreateMap<TblMgainPlotData, MGainPlotDetailsDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.ProjectName, opt => opt.MapFrom(src => src.TblProjectMaster.Name))
                .ForMember(x => x.PlotNo, opt => opt.MapFrom(src => src.TblPlotMaster.PlotNo))
                .ForMember(x => x.AllocatedSqFt, opt => opt.MapFrom(src => src.AllocatedSqFt))
                .ForMember(x => x.AllocatedAmt, opt => opt.MapFrom(src => src.AllocatedAmt))
                .ForMember(x => x.MgainId, opt => opt.MapFrom(src => src.MgainId))
                .ForMember(x => x.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(x => x.PlotId, opt => opt.MapFrom(src => src.PlotId));


        }
    }
}
