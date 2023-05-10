using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Model
{
    public class UserMaster
    {
        [Key]
        public int User_Id { get; set; }
        public Nullable<int> Cat_Id { get; set; }
        public Nullable<int> User_SponId { get; set; }
        public Nullable<int> User_ParentId { get; set; }
        public string? User_Name { get; set; }
        public string? User_Pan { get; set; }
        public DateTime User_Doj { get; set; }
        public string? User_Mobile { get; set; }
        public string? User_Email { get; set; }
        public string? User_Addr { get; set; }
        public string? User_Pin { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string? User_Uname { get; set; }
        public string? User_Passwd { get; set; }
        public Nullable<bool> User_IsActive { get; set; }
        public Nullable<int> User_PurposeId { get; set; }
        public string? User_ProfilePhoto { get; set; }
        public string? User_PromoCode { get; set; }
        public string? User_SubCategory { get; set; }
        public string? User_GstNo { get; set; }
        public string? User_FcmId { get; set; }
        public Nullable<DateTime> User_FcmLastUpdateDateTime { get; set; }
        public Nullable<DateTime> User_Dob { get; set; }
        public string? User_Aadhar { get; set; }
        public Nullable<int> User_AccountType { get; set; }
        public Nullable<bool> User_fastTrack { get; set; }
        public Nullable<bool> User_WbcActive { get; set; }
        public Nullable<int> TotalCountofAddContact { get; set; }
        public string? User_Deviceid { get; set; }
        public Nullable<bool> User_TermAndCondition { get; set; }
        public Nullable<int> Family_Id { get; set; }
        public string? User_NjName { get; set; }
        public Nullable<DateTime> FastTrackActivationDate { get; set; }

        [ForeignKey(nameof(Cat_Id))]
        public virtual UserCategoryMaster UserCategoryMaster { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual CountryMaster CountryMaster { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual StateMaster StateMaster { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual CityMaster CityMaster { get; set; }

        public UserMaster()
        {

        }

        public UserMaster(int? cat_Id, int? user_SponId, int? user_ParentId, string? user_Name, string? user_Pan, DateTime user_Doj, string? user_Mobile, string? user_Email, string? user_Addr, string? user_Pin, int? user_CountryId, int? user_StateId, int? user_CityId, string? user_Uname, string? user_Passwd, bool? user_IsActive, int? user_PurposeId, string? user_ProfilePhoto, string? user_PromoCode, string? user_SubCategory, string? user_GstNo, string? user_FcmId, DateTime? user_Dob, string? user_Aadhar, int? user_AccountType, bool? user_fastTrack, bool? user_WbcActive, string? user_Deviceid, bool? user_TermAndCondition, int? family_Id, string? user_NjName)
        {
            Cat_Id = cat_Id;
            User_SponId = user_SponId;
            User_ParentId = user_ParentId;
            User_Name = user_Name;
            User_Pan = user_Pan;
            User_Doj = user_Doj;
            User_Mobile = user_Mobile;
            User_Email = user_Email;
            User_Addr = user_Addr;
            User_Pin = user_Pin;
            CountryId = user_CountryId;
            StateId = user_StateId;
            CityId = user_CityId;
            User_Uname = user_Uname;
            User_Passwd = user_Passwd;
            User_IsActive = user_IsActive;
            User_PurposeId = user_PurposeId;
            User_ProfilePhoto = user_ProfilePhoto;
            User_PromoCode = user_PromoCode;
            User_SubCategory = user_SubCategory;
            User_GstNo = user_GstNo;
            User_FcmId = user_FcmId;
            User_Dob = user_Dob;
            User_Aadhar = user_Aadhar;
            User_AccountType = user_AccountType;
            User_fastTrack = user_fastTrack;
            User_WbcActive = user_WbcActive;
            User_Deviceid = user_Deviceid;
            User_TermAndCondition = user_TermAndCondition;
            Family_Id = family_Id;
            User_NjName = user_NjName;
        }


        public void UpdateUserMaster(int? cat_Id, int? user_SponId, int? user_ParentId, string? user_Name, string? user_Pan, DateTime user_Doj, string? user_Mobile, string? user_Email, string? user_Addr, string? user_Pin, int? user_CountryId, int? user_StateId, int? user_CityId, string? user_Uname, string? user_Passwd, bool? user_IsActive, int? user_PurposeId, string? user_ProfilePhoto, string? user_PromoCode, string? user_SubCategory, string? user_GstNo, DateTime? user_Dob, string? user_Aadhar, int? user_AccountType, bool? user_fastTrack, bool? user_WbcActive, string? user_Deviceid, bool? user_TermAndCondition, int? family_Id, string? user_NjName)
        {
            Cat_Id = cat_Id;
            User_SponId = user_SponId;
            User_ParentId = user_ParentId;
            User_Name = user_Name;
            User_Pan = user_Pan;
            User_Doj = user_Doj;
            User_Mobile = user_Mobile;
            User_Email = user_Email;
            User_Addr = user_Addr;
            User_Pin = user_Pin;
            CountryId = user_CountryId;
            StateId = user_StateId;
            CityId = user_CityId;
            User_Uname = user_Uname;
            User_Passwd = user_Passwd;
            User_IsActive = user_IsActive;
            User_PurposeId = user_PurposeId;
            User_ProfilePhoto = user_ProfilePhoto;
            User_PromoCode = user_PromoCode;
            User_SubCategory = user_SubCategory;
            User_GstNo = user_GstNo;
            User_Dob = user_Dob;
            User_Aadhar = user_Aadhar;
            User_AccountType = user_AccountType;
            User_fastTrack = user_fastTrack;
            User_WbcActive = user_WbcActive;
            User_Deviceid = user_Deviceid;
            User_TermAndCondition = user_TermAndCondition;
            Family_Id = family_Id;
            User_NjName = user_NjName;
        }
    }
}
