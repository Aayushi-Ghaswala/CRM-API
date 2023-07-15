using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblGoldPoint
    {
        public int GpId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Credit { get; set; } = 0;
        public int? Debit { get; set; } = 0;
        public int? Userid { get; set; }
        public string? Type { get; set; }
        public int? PointCategory { get; set; }
        public int? Vendorid { get; set; }

        [ForeignKey(nameof(Userid))]
        public virtual TblUserMaster TblUserMaster { get; set; }
        [ForeignKey(nameof(PointCategory))]
        public virtual TblGoldPointCategory? TblGoldPointCategory { get; set; }
    }
}
