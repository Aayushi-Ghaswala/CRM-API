using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblExcelimportUsermaster
    {
        public int UserId { get; set; }
        public int? CatId { get; set; }
        public int? UserSponid { get; set; }
        public int? UserParentid { get; set; }
        public string? UserName { get; set; }
        public string? UserPan { get; set; }
        public DateTime? UserDoj { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public string? UserAddr { get; set; }
        public string? UserPin { get; set; }
        public string? UserCountryname { get; set; }
        public string? UserStatename { get; set; }
        public string? UserCityname { get; set; }
        public int? UserCountryid { get; set; }
        public int? UserStateid { get; set; }
        public int? UserCityid { get; set; }
        public string? UserUname { get; set; }
        public string? UserPasswd { get; set; }
        public bool? UserIsactive { get; set; }
        public int? UserPurposeid { get; set; }
        public string? UserProfilephoto { get; set; }
        public string? UserPromocode { get; set; }
        public string? UserSubcategory { get; set; }
        public string? UserGstno { get; set; }
        public string? UserFcmid { get; set; }
        public DateTime? UserFcmlastupdaetime { get; set; }
        public DateTime? UserDob { get; set; }
        public string? UserAadhar { get; set; }
        public int? UserAccounttype { get; set; }
        public bool? UserFasttrack { get; set; }
        public bool? UserWbcActive { get; set; }
        public int? Totalcountofaddcontact { get; set; }
        public string? UserDeviceid { get; set; }
        public bool? UserTermandcondition { get; set; }
        public int? FamilyId { get; set; }
        public string? UserNjname { get; set; }
    }
}
