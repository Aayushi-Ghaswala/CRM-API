using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblUserMaster
    {
        public TblUserMaster()
        {
            TblFasttrackLedgers = new HashSet<TblFasttrackLedger>();
            TblFolioMasters = new HashSet<TblFolioMaster>();
            TblGoldReferrals = new HashSet<TblGoldReferral>();
            TblMgainInvesments = new HashSet<TblMgainInvesment>();
            TblRealEastateReviews = new HashSet<TblRealEastateReview>();
            TblReferralMasters = new HashSet<TblReferralMaster>();
        }

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
        public DateTime? FastTrackActivationDate { get; set; }

        [ForeignKey(nameof(CatId))]
        public virtual TblUserCategoryMaster TblUserCategoryMaster { get; set; }
        [ForeignKey(nameof(UserCountryid))]
        public virtual TblCountryMaster TblCountryMaster { get; set; }
        [ForeignKey(nameof(UserStateid))]
        public virtual TblStateMaster TblStateMaster { get; set; }
        [ForeignKey(nameof(UserCityid))]
        public virtual TblCityMaster TblCityMaster { get; set; }

        public virtual ICollection<TblFasttrackLedger> TblFasttrackLedgers { get; set; }
        public virtual ICollection<TblFolioMaster> TblFolioMasters { get; set; }
        public virtual ICollection<TblGoldReferral> TblGoldReferrals { get; set; }
        public virtual ICollection<TblMgainInvesment> TblMgainInvesments { get; set; }
        public virtual ICollection<TblRealEastateReview> TblRealEastateReviews { get; set; }
        public virtual ICollection<TblReferralMaster> TblReferralMasters { get; set; }

        public TblUserMaster(int? cat_Id, int? user_SponId, int? user_ParentId, string? user_Name, string? user_Pan, DateTime user_Doj, string? user_Mobile, string? user_Email, string? user_Addr, string? user_Pin, int? user_CountryId, int? user_StateId, int? user_CityId, string? user_Uname, string? user_Passwd, bool? user_IsActive, string? user_ProfilePhoto, string? user_PromoCode, string? user_SubCategory, string? user_GstNo, string? user_FcmId, DateTime? user_Dob, string? user_Aadhar, int? user_AccountType, bool? user_fastTrack, bool? user_WbcActive, string? user_Deviceid, bool? user_TermAndCondition, int? family_Id, string? user_NjName)
        {
            CatId = cat_Id;
            UserSponid = user_SponId;
            UserParentid = user_ParentId;
            UserName = user_Name;
            UserPan = user_Pan;
            UserDoj = user_Doj;
            UserMobile = user_Mobile;
            UserEmail = user_Email;
            UserAddr = user_Addr;
            UserPin = user_Pin;
            UserCountryid = user_CountryId;
            UserStateid = user_StateId;
            UserCityid = user_CityId;
            UserUname = user_Uname;
            UserPasswd = user_Passwd;
            UserIsactive = user_IsActive;
            UserProfilephoto = user_ProfilePhoto;
            UserPromocode = user_PromoCode;
            UserSubcategory = user_SubCategory;
            UserGstno = user_GstNo;
            UserFcmid = user_FcmId;
            UserDob = user_Dob;
            UserAadhar = user_Aadhar;
            UserAccounttype = user_AccountType;
            UserFasttrack = user_fastTrack;
            UserWbcActive = user_WbcActive;
            UserDeviceid = user_Deviceid;
            UserTermandcondition = user_TermAndCondition;
            FamilyId = family_Id;
            UserNjname = user_NjName;
        }

        public void UpdateUserMaster(int? cat_Id, int? user_SponId, int? user_ParentId, string? user_Name, string? user_Pan, DateTime user_Doj, string? user_Mobile, string? user_Email, string? user_Addr, string? user_Pin, int? user_CountryId, int? user_StateId, int? user_CityId, string? user_Uname, string? user_Passwd, bool? user_IsActive, string? user_ProfilePhoto, string? user_PromoCode, string? user_SubCategory, string? user_GstNo, DateTime? user_Dob, string? user_Aadhar, int? user_AccountType, bool? user_fastTrack, bool? user_WbcActive, string? user_Deviceid, bool? user_TermAndCondition, int? family_Id, string? user_NjName)
        {
            CatId = cat_Id;
            UserSponid = user_SponId;
            UserParentid = user_ParentId;
            UserName = user_Name;
            UserPan = user_Pan;
            UserDoj = user_Doj;
            UserMobile = user_Mobile;
            UserEmail = user_Email;
            UserAddr = user_Addr;
            UserPin = user_Pin;
            UserCountryid = user_CountryId;
            UserStateid = user_StateId;
            UserCityid = user_CityId;
            UserUname = user_Uname;
            UserPasswd = user_Passwd;
            UserIsactive = user_IsActive;
            UserProfilephoto = user_ProfilePhoto;
            UserPromocode = user_PromoCode;
            UserSubcategory = user_SubCategory;
            UserGstno = user_GstNo;
            UserDob = user_Dob;
            UserAadhar = user_Aadhar;
            UserAccounttype = user_AccountType;
            UserFasttrack = user_fastTrack;
            UserWbcActive = user_WbcActive;
            UserDeviceid = user_Deviceid;
            UserTermandcondition = user_TermAndCondition;
            FamilyId = family_Id;
            UserNjname = user_NjName;
        }
    }
}
