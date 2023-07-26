namespace CRM_api.DataAccess.Models
{
    public partial class TblCompanyMaster
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobileno { get; set; }
        public string? Address { get; set; }
        public string? OfficeMobileno { get; set; }
        public string? OfficeEmail { get; set; }
        public string? FactoryAdd { get; set; }
        public string? FactoryEmail { get; set; }
        public string? FactoryMobileno { get; set; }
        public string? Type { get; set; }
        public string? GstNo { get; set; }
        public DateTime? GstRegDate { get; set; }
        public bool? Isdeleted { get; set; }

        public virtual ICollection<TblAccountMaster> TblAccountMasters { get; set; }
    }
}
