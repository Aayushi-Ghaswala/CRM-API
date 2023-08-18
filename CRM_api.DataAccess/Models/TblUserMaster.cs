using System.ComponentModel.DataAnnotations;
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

        [Key]
        public int UserId { get; set; }
        public int? CatId { get; set; }
        public int? UserSponid { get; set; }
        public int? UserParentid { get; set; }
        public string? UserName { get; set; }
        public string? UserPan { get; set; }
        public DateTime? UserDoj { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public string? UserWorkemail { get; set; }
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
        public virtual TblUserCategoryMaster? TblUserCategoryMaster { get; set; }
        [ForeignKey(nameof(UserCountryid))]
        public virtual TblCountryMaster? TblCountryMaster { get; set; }
        [ForeignKey(nameof(UserStateid))]
        public virtual TblStateMaster? TblStateMaster { get; set; }
        [ForeignKey(nameof(UserCityid))]
        public virtual TblCityMaster? TblCityMaster { get; set; }
        [ForeignKey(nameof(UserParentid))]
        public virtual TblUserMaster? ParentName { get; set; }
        [ForeignKey(nameof(UserSponid))]
        public virtual TblUserMaster? SponserName { get; set; }

        public virtual ICollection<TblFasttrackLedger>? TblFasttrackLedgers { get; set; }
        public virtual ICollection<TblFolioMaster>? TblFolioMasters { get; set; }
        public virtual ICollection<TblGoldReferral>? TblGoldReferrals { get; set; }
        public virtual ICollection<TblMgainInvesment>? TblMgainInvesments { get; set; }
        public virtual ICollection<TblRealEastateReview>? TblRealEastateReviews { get; set; }
        public virtual ICollection<TblReferralMaster>? TblReferralMasters { get; set; }
        public virtual ICollection<TblRoleAssignment>? TblRoleAssignments { get; set; }
        public virtual ICollection<TblMgaindetail>? TblMgaindetails { get; set; }
        public virtual ICollection<TblMftransaction>? TblMftransactions { get; set; }
        public virtual ICollection<TblLoanMaster>? TblLoanmasters { get; set;}
        public virtual ICollection<TblInsuranceclient>? TblInsuranceclients { get; set; }
        public virtual ICollection<TblAccountTransaction>? TblAccounttransactions { get; set; }
        public virtual ICollection<TblAccountMaster>? TblAccountMasters { get; set; }
    }
}
