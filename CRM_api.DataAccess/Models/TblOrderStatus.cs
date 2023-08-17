namespace CRM_api.DataAccess.Models
{
    public partial class TblOrderStatus
    {
        public int Id { get; set; }
        public string? Statusname { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
