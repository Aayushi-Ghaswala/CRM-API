using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblInsuranceclient
    {
        public int Id { get; set; }
        public string? InsPlantype { get; set; }
        public string? InsPlan { get; set; }
        public string? InsPolicy { get; set; }
        public string? InsUsername { get; set; }
        public DateTime? InsDuedate { get; set; }
        public decimal? PremiumAmount { get; set; }
        public string? InsEmail { get; set; }
        public string? InsPan { get; set; }
        public string? InsMobile { get; set; }
        public int? InsUserid { get; set; }
        public int? InvType { get; set; }
        public int? InvSubtype { get; set; }
        public int? InsAmount { get; set; }
        public string? InsNewpolicy { get; set; }
        public int? InsPlantypeId { get; set; }
        public int? Companyid { get; set; }
        public DateTime? InsStartdate { get; set; }
        public int? InsTerm { get; set; }
        public string? InsFrequency { get; set; }
        public DateTime? InsPremiumRmdDate { get; set; }
        public bool? IsSendForReview { get; set; }
        public bool? IsKathrough { get; set; }
        public bool? IsEmailReminder { get; set; }
        public bool? IsNotification { get; set; }
        public bool? IsSmsReminder { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(InsUserid))]
        public virtual TblUserMaster TblUserMaster { get; set; }
        [ForeignKey(nameof(InvType))]
        public virtual TblInvesmentType TblInvesmentType { get; set; }
        [ForeignKey(nameof(InvSubtype))]
        public virtual TblSubInvesmentType TblSubInvesmentType { get; set; }
        [ForeignKey(nameof(Companyid))]
        public virtual TblInsuranceCompanylist TblInsuranceCompanylist { get; set; }

        public virtual TblInsuranceTypeMaster? TblInsuranceTypeMaster { get; set; }
    }
}
