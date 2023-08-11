using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblDesignationMaster
    {
        public int DesignationId { get; set; }
        public int? ParentDesignationId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }

        [ForeignKey(nameof(ParentDesignationId))]
        public virtual TblDesignationMaster TblParentDesignationMaster { get; set; }
    }
}
