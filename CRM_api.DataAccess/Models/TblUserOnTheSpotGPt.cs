using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblUserOnTheSpotGP
    {
        public int Id { get; set; }
        public int WbcSchemeId { get; set; }
        public int UserId { get; set; }
        public int? Credit { get; set; } = 0;
        public int? Debit { get; set; } = 0;
        public string WbcTypeName { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey(nameof(WbcSchemeId))]
        public virtual TblWbcSchemeMaster TblWbcSchemeMaster { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual TblUserMaster TblUserMaster { get; set; }
    }
}
