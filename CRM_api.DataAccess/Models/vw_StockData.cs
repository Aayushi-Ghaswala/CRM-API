using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Models
{
    [Keyless]
    public class vw_StockData
    {
        public DateTime? StDate { get; set; }
        public string? StClientname { get; set; }
        public string? StScripname { get; set; }
        public string? StType { get; set; }
        public int? StQty { get; set; }
        public decimal? StNetsharerate { get; set; }
        public decimal? StNetcostvalue { get; set; }
    }
}
