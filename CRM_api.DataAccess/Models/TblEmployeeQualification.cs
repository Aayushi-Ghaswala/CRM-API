using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblEmployeeQualification
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string? Year { get; set; }
        public string? Degree { get; set; }
        public decimal? Score { get; set; }
        public string? UniCollege { get; set; }

        [ForeignKey(nameof(EmpId))]
        public virtual TblEmployeeMaster TblEmployeeMaster { get; set; }
    }
}
