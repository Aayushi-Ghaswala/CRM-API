using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblWbcSchemeMaster
    {
        public int Id { get; set; }
        public int? WbcTypeId { get; set; }
        public int? ParticularsId { get; set; }
        public int? ParticularsSubTypeId { get; set; }
        public bool? IsRedeemable { get; set; }
        public string? Business { get; set; }
        public int? GoldPoint { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? On_the_spot_GP { get; set; }
        public bool IsParentAllocation { get; set; }

        [ForeignKey(nameof(WbcTypeId))]
        public virtual TblWbcTypeMaster? TblWbcTypeMaster { get; set; }
        [ForeignKey(nameof(ParticularsId))]
        public virtual TblSubInvesmentType? TblSubInvesmentType { get; set; }
        [ForeignKey(nameof(ParticularsSubTypeId))]
        public virtual TblSubsubInvType? TblSubsubInvType { get; set; }
    }
}
