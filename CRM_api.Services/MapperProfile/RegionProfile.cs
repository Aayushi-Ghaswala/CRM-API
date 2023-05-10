using AutoMapper;
using CRM_api.DataAccess.Model;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.MapperProfile
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<CountryMaster, CountryMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.Country_Name, opt => opt.MapFrom(src => src.Country_Name))
                .ForMember(c => c.Isdcode, opt => opt.MapFrom(src => src.Isdcode))
                .ForMember(c => c.Icon, opt => opt.MapFrom(src => src.Icon));

            CreateMap<StateMaster, StateMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.State_Name, opt => opt.MapFrom(src => src.State_Name));

            CreateMap<CityMaster, CityMasterDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.City_Name, opt => opt.MapFrom(src => src.City_Name));
        }
    }
}
