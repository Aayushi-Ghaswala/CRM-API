namespace CRM_api.DataAccess.Models
{
    public partial class TblFinancialYearMaster
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public DateTime? Startdate { get; set; }
        public DateTime? Enddate { get; set; }
        public bool? Isdeleted { get; set; }

        public virtual ICollection<TblAccountOpeningBalance> AccountOpeningBalances { get; set; }
    }
}
