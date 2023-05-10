using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class Usercleantable
    {
        public int? UserId { get; set; }
        public int? CatId { get; set; }
        public int? UserParentid { get; set; }
        public int? UserSponid { get; set; }
        public string? UserFirstname { get; set; }
        public string? UserLastname { get; set; }
        public string? UserMiddlename { get; set; }
        public string? UserName { get; set; }
        public string? UserUname { get; set; }
        public string? UserPasswd { get; set; }
        public string? UserPromocode { get; set; }
        public int? UserSubcategory { get; set; }
        public string? UserPan { get; set; }
        public DateTime? UserDob { get; set; }
        public string? UserAddline1 { get; set; }
        public string? UserAddline2 { get; set; }
        public string? UserAddline3 { get; set; }
        public string? UserAdd { get; set; }
        public string? UserCityname { get; set; }
        public string? UserStatename { get; set; }
        public int? UserCityid { get; set; }
        public int? UserStateid { get; set; }
        public int? UserCountryid { get; set; }
        public int? UserPin { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public string? UseAdhar { get; set; }
    }
}
