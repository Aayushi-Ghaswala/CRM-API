using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMftransaction
    {

        public int Trnid { get; set; }
        public string? Foliono { get; set; }
        public string? Schemename { get; set; }
        public int? Userid { get; set; }
        public string? Username { get; set; }
        public string? Transactiontype { get; set; }
        public string? Tradeno { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Noofunit { get; set; }
        public double? Nav { get; set; }
        public decimal? Invamount { get; set; }
        public decimal? Unitbalance { get; set; }
        public int? SchemeId { get; set; }
        public string? Notes { get; set; }

        [ForeignKey(nameof(Userid))]
        public virtual TblUserMaster TblUserMaster { get; set; }

        [ForeignKey(nameof(SchemeId))]
        public virtual TblMfSchemeMaster TblMfSchemeMaster { get; set; }
    }
}
