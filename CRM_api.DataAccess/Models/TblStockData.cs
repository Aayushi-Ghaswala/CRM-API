using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblStockData
    {
        public int Id { get; set; }
        public int? StBranch { get; set; }
        public string? StClientcode { get; set; }
        public string? StClientname { get; set; }
        public string? StScripname { get; set; }
        public string? StTransactionDetails { get; set; }
        public string? StSettno { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StDate { get; set; }
        public string? StType { get; set; }
        public int? StQty { get; set; }
        public decimal? StRate { get; set; }
        public decimal? StBrokerage { get; set; }
        public decimal? StNetrate { get; set; } = 0;
        public decimal? StNetvalue { get; set; } = 0;
        public decimal? StCostpershare { get; set; } = 0;
        public decimal? StCostvalue { get; set; } = 0;
        public decimal? StNetsharerate { get; set; } = 0;
        public decimal? StNetcostvalue { get; set; } = 0;
        public int? Userid { get; set; }
        public string? FirmName { get; set; }
        public string? FileType { get; set; }

        [ForeignKey(nameof(Userid))]
        public virtual TblUserMaster TblUserMaster { get; set; }
    }
}
