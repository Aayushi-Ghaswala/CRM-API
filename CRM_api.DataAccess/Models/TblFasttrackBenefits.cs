using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblFasttrackBenefits
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public decimal Basic { get; set; }
        public decimal Silver { get; set; }
        public decimal Gold { get; set; }
        public decimal Platinum { get; set; }
        public decimal Diamond { get; set; }
        public int InvTypeId { get; set; }
        public bool IsParentAllocation { get; set; }

        [ForeignKey(nameof(InvTypeId))]
        public virtual TblInvesmentType TblInvesmentType { get; set; }
    }
}