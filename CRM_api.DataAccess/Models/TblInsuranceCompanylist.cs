namespace CRM_api.DataAccess.Models
{
    public partial class TblInsuranceCompanylist
    {
        public int InsuranceCompanyid { get; set; }
        public int? InsuranceCompanytypeid { get; set; }
        public string? InsuranceCompanyname { get; set; }

        public virtual TblInsurancetype? InsuranceCompanytype { get; set; }
    }
}
