using AutoMapper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.MapperProfile
{
    public class UserMasterProfile : Profile
    {
        public UserMasterProfile()
        {
            CreateMap<TblUserCategoryMaster, UserCategoryDto>();
            CreateMap<AddUserCategoryDto, TblUserCategoryMaster>();
            CreateMap<UpdateUserCategoryDto, TblUserCategoryMaster>();

            CreateMap<TblUserMaster, UserMasterDto>();
            CreateMap<Pagination, PaginationDto>();
            CreateMap<Response<TblUserMaster>, ResponseDto<UserMasterDto>>();

            CreateMap<Response<TblUserCategoryMaster>, ResponseDto<UserCategoryDto>>();

            CreateMap<UpdateUserMasterDto, TblUserMaster>();
            CreateMap<AddUserMasterDto, TblUserMaster>()
                .AfterMap((dto, user) =>
                {
                    user.UserFcmid = Guid.NewGuid().ToString();
                    if (dto.UserfastTrack == true)
                        user.FastTrackActivationDate = DateTime.Now;
                    user.UserIsactive = true;
                });

            CreateMap<TblUserMaster, UserMasterCSVDto>()
                .ForMember(user => user.Parent, opt => opt.MapFrom(src => src.ParentName.UserName))
                .ForMember(user => user.Sponser, opt => opt.MapFrom(src => src.SponserName.UserName))
                .ForMember(user => user.UserDoj, opt => opt.MapFrom(src => src.UserDoj.Value.ToShortDateString()))
                .ForMember(user => user.Country, opt => opt.MapFrom(src => src.TblCountryMaster.CountryName))
                .ForMember(user => user.State, opt => opt.MapFrom(src => src.TblStateMaster.StateName))
                .ForMember(user => user.City, opt => opt.MapFrom(src => src.TblCityMaster.CityName))
                .ForMember(user => user.UserFcmlastupdaetime, opt => opt.MapFrom(src => src.UserFcmlastupdaetime.Value.ToShortDateString()))
                .ForMember(user => user.UserDob, opt => opt.MapFrom(src => src.UserDob.Value.ToShortDateString()))
                .ForMember(user => user.FastTrackActivationDate, opt => opt.MapFrom(src => src.FastTrackActivationDate.Value.ToShortDateString()));

            CreateMap<TblFamilyMember, FamilyMemberDto>();
            CreateMap<Response<TblFamilyMember>, ResponseDto<FamilyMemberDto>>();
        }
    }
}
