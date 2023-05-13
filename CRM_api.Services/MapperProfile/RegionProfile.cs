using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.MapperProfile
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<TblCountryMaster, CountryMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.CountryId));
            CreateMap<TblStateMaster, StateMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.StateId));

            CreateMap<TblCityMaster, CityMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.CityId));

            CreateMap<Response<TblCountryMaster>, ResponseDto<CountryMasterDto>>();
            CreateMap<Response<TblStateMaster>, ResponseDto<StateMasterDto>>();
            CreateMap<Response<TblCityMaster>, ResponseDto<CityMasterDto>>();
        }
    }
}
