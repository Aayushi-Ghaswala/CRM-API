using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblSubInvesmentType
    {
        public int Id { get; set; }
        public string? InvestmentType { get; set; }
        public int? InvesmentTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(InvesmentTypeId))]
        public virtual TblInvesmentType InvesmentType { get; set; } = null!;
    }
}
