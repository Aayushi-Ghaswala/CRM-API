using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Models
{
    [Keyless]
    public partial class GetTopTenSchemeByInvestment
    {
        public string Schemename { get; set; }
        public decimal TotalInvestMentAmount { get; set; }
    }
}
