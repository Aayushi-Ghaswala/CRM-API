using AutoMapper;
using CRM_api.DataAccess.Model;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.MapperProfile
{
    public class UserMasterProfile : Profile
    {
        public UserMasterProfile()
        {
            CreateMap<UserMaster, DisplayUserMasterDto>()
                .ForMember(um => um.User_Id, opt => opt.MapFrom(src => src.User_Id))
                .ForMember(um => um.User_Doj, opt => opt.MapFrom(src => src.User_Doj))
                .ForMember(um => um.User_UName, opt => opt.MapFrom(src => src.User_Uname))
                .ForMember(um => um.User_Name, opt => opt.MapFrom(src => src.User_Name))
                .ForMember(um => um.User_Mobile, opt => opt.MapFrom(src => src.User_Mobile))
                .ForMember(um => um.User_Email, opt => opt.MapFrom(src => src.User_Email));

            CreateMap<UserMaster, GetUserMasterForUpdateDto>()
                .ForMember(um => um.Id, opt => opt.MapFrom(src => src.User_Id))
                .ForMember(um => um.Category, opt => opt.MapFrom(src => src.UserCategoryMaster.Cat_Name))
                .ForMember(um => um.SponId, opt => opt.MapFrom(src => src.User_SponId))
                .ForMember(um => um.ParentId, opt => opt.MapFrom(src => src.User_ParentId))
                .ForMember(um => um.Name, opt => opt.MapFrom(src => src.User_Name))
                .ForMember(um => um.Pan, opt => opt.MapFrom(src => src.User_Pan))
                .ForMember(um => um.Doj, opt => opt.MapFrom(src => src.User_Doj))
                .ForMember(um => um.Mobile, opt => opt.MapFrom(src => src.User_Mobile))
                .ForMember(um => um.Email, opt => opt.MapFrom(src => src.User_Email))
                .ForMember(um => um.Addr, opt => opt.MapFrom(src => src.User_Addr))
                .ForMember(um => um.Pin, opt => opt.MapFrom(src => src.User_Pin))
                .ForMember(um => um.Country, opt => opt.MapFrom(src => src.CountryMaster.Country_Name))
                .ForMember(um => um.State, opt => opt.MapFrom(src => src.StateMaster.State_Name))
                .ForMember(um => um.City, opt => opt.MapFrom(src => src.CityMaster.City_Name))
                .ForMember(um => um.Uname, opt => opt.MapFrom(src => src.User_Uname))
                .ForMember(um => um.Passwd, opt => opt.MapFrom(src => src.User_Passwd))
                .ForMember(um => um.IsActive, opt => opt.MapFrom(src => src.User_IsActive))
                .ForMember(um => um.PurposeId, opt => opt.MapFrom(src => src.User_PurposeId))
                .ForMember(um => um.ProfilePhoto, opt => opt.MapFrom(src => src.User_ProfilePhoto))
                .ForMember(um => um.PromoCode, opt => opt.MapFrom(src => src.User_PromoCode))
                .ForMember(um => um.SubCategory, opt => opt.MapFrom(src => src.User_SubCategory))
                .ForMember(um => um.GstNo, opt => opt.MapFrom(src => src.User_GstNo))
                .ForMember(um => um.FcmId, opt => opt.MapFrom(src => src.User_FcmId))
                .ForMember(um => um.FcmLastUpdateDateTime, opt => opt.MapFrom(src => src.User_FcmLastUpdateDateTime))
                .ForMember(um => um.Dob, opt => opt.MapFrom(src => src.User_Dob))
                .ForMember(um => um.Aadhar, opt => opt.MapFrom(src => src.User_Aadhar))
                .ForMember(um => um.AccountType, opt => opt.MapFrom(src => src.User_AccountType))
                .ForMember(um => um.fastTrack, opt => opt.MapFrom(src => src.User_fastTrack))
                .ForMember(um => um.WbcActive, opt => opt.MapFrom(src => src.User_WbcActive))
                .ForMember(um => um.TotalCountofAddContact, opt => opt.MapFrom(src => src.TotalCountofAddContact))
                .ForMember(um => um.Deviceid, opt => opt.MapFrom(src => src.User_Deviceid))
                .ForMember(um => um.TermAndCondition, opt => opt.MapFrom(src => src.User_TermAndCondition))
                .ForMember(um => um.Family_Id, opt => opt.MapFrom(src => src.Family_Id))
                .ForMember(um => um.NjName, opt => opt.MapFrom(src => src.User_NjName))
                .ForMember(um => um.FastTrackActivationDate, opt => opt.MapFrom(src => src.FastTrackActivationDate));

            CreateMap<UserCategoryMaster, UserCategoryDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Cat_Id))
                .ForMember(c => c.Cat_Name, opt => opt.MapFrom(src => src.Cat_Name));
        }
    }
}
