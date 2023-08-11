using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblEmployeeExperience
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CompanyName { get; set; }

        [ForeignKey(nameof(EmpId))]
        public virtual TblEmployeeMaster TblEmployeeMaster { get; set; }
    }
}
