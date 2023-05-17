using System.ComponentModel.DataAnnotations;

namespace CRM_api.DataAccess.Models
{
    public partial class TblStockData
    {
        public int Id { get; set; }
        public int? StBranch { get; set; }
        public string? StClientcode { get; set; }
        public string? StClientname { get; set; }
        public string? StScripname { get; set; }
        public string? StSettno { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StDate { get; set; }
        public string? StType { get; set; }
        public int? StQty { get; set; }
        public decimal? StRate { get; set; }
        public decimal? StBrokerage { get; set; }
        public decimal? StNetrate { get; set; }
        public decimal? StNetvalue { get; set; }
        public decimal? StCostpershare { get; set; }
        public decimal? StCostvalue { get; set; }
        public decimal? StNetsharerate { get; set; }
        public decimal? StNetcostvalue { get; set; }
        public int? Userid { get; set; }
    }
}
