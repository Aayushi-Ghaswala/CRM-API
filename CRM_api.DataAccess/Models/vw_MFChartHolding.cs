using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Models
{
    [Keyless]
    public class vw_MFChartHolding
    {
        public string? Username { get; set; }
        public string? Schemename { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Noofunit { get; set; }
        public string? Transactiontype { get; set; }
    }
}
