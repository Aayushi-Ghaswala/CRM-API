using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblSubsubInvType
    {
        public int Id { get; set; }
        public string? SubInvType { get; set; }
        public int? SubInvTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        [ForeignKey(nameof(SubInvTypeId))]
        public virtual TblSubInvesmentType TblSubInvesmentType { get; set; }
    }
}
