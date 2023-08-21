using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblProjectTypeDetail
    {
        public int Id { get; set; }
        public int? ProjectTypeId { get; set; }
        public string? ProjectTypeDetail { get; set; }
        [ForeignKey(nameof(ProjectTypeId))]
        public virtual TblProjectTypeMaster TblProjectTypeMaster { get; set; }
    }
}
