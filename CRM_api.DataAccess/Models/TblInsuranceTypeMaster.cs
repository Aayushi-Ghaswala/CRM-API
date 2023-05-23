namespace CRM_api.DataAccess.Models
{
    public partial class TblInsuranceTypeMaster
    {
        public TblInsuranceTypeMaster()
        {
            TblInsuranceclients = new HashSet<TblInsuranceclient>();
        }

        public int InsPlantypeId { get; set; }
        public string? InsPlanType { get; set; }

        public virtual ICollection<TblInsuranceclient> TblInsuranceclients { get; set; }
    }
}
