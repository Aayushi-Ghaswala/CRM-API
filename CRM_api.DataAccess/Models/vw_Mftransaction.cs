using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Models
{
    [Keyless]
    public class vw_Mftransaction
    {
        public string? Username { get; set; }
        public decimal? Invamount { get; set; }
        public string? Transactiontype { get; set; }
        public DateTime? Date { get; set; }
    }
}
