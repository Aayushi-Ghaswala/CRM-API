using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Dtos.AddDataDto
{
    public class AddUserMasterDto
    {
        public Nullable<int> CatId { get; set; }
        public Nullable<int> UserSponId { get; set; }
        public Nullable<int> UserParentId { get; set; }
        public string? UserName { get; set; }
        public string? UserPan { get; set; }
        public Nullable<DateTime> UserDoj { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public string? UserWorkemail { get; set; }
        public string? UserAddr { get; set; }
        public string? UserPin { get; set; }
        public Nullable<int> UserCountryId { get; set; }
        public Nullable<int> UserStateId { get; set; }
        public Nullable<int> UserCityId { get; set; }
        public string? UserUname { get; set; }
        public string? UserPasswd { get; set; }
        public string? UserProfilePhoto { get; set; }
        public string? UserPromoCode { get; set; }
        public string? UserSubCategory { get; set; }
        public string? UserGstNo { get; set; }
        public Nullable<DateTime> UserDob { get; set; }
        public string? UserAadhar { get; set; }
        public Nullable<bool> UserfastTrack { get; set; }
        public Nullable<bool> UserWbcActive { get; set; }
        public Nullable<bool> UserTermAndCondition { get; set; }

        public IFormFile? UserPanFile { get; set; }
        public IFormFile? UserAadharFile { get; set; }
        public IFormFile? UserImageFile { get; set; }
    }
}
