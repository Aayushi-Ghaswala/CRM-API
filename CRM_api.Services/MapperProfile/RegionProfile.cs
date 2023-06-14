using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<AddCountryDto, TblCountryMaster>()
                .AfterMap((dto, model) =>
                {
                    model.IsDeleted = false;
                });
            CreateMap<AddStateDto, TblStateMaster>()
                .AfterMap((dto, model) =>
                {
                    model.IsDeleted = false;
                });
            CreateMap<AddCityDto, TblCityMaster>()
                .AfterMap((dto, model) =>
                {
                    model.IsDeleted = false;
                });

            CreateMap<UpdateCountryDto, TblCountryMaster>();
            CreateMap<UpdateStateDto, TblStateMaster>();
            CreateMap<UpdateCityDto, TblCityMaster>();

            CreateMap<TblCountryMaster, CountryMasterDto>();
            CreateMap<TblStateMaster, StateMasterDto>();
            CreateMap<TblCityMaster, CityMasterDto>();

            CreateMap<Response<TblCountryMaster>, ResponseDto<CountryMasterDto>>();
            CreateMap<Response<TblStateMaster>, ResponseDto<StateMasterDto>>();
            CreateMap<Response<TblCityMaster>, ResponseDto<CityMasterDto>>();
        }
    }
}
