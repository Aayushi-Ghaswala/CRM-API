namespace CRM_api.DataAccess.Models
{
    public partial class TblGoldReferral
    {
        public int RefId { get; set; }
        public string? RefName { get; set; }
        public string? RefMobile { get; set; }
        public string? RefEmail { get; set; }
        public int? CityId { get; set; }
        public int? RefById { get; set; }
        public DateTime? RefDate { get; set; }
        public string? RefStatus { get; set; }
        public bool? RefIsactive { get; set; }
        public string? RefReason { get; set; }
        public string? RefType { get; set; }
        public string? RefRel { get; set; }
        public int? Totalcountofaddcontact { get; set; }

        public virtual TblUserMaster? RefBy { get; set; }
    }
}
