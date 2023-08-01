namespace CRM_api.DataAccess.Models
{
    public partial class TblPaymentTypeMaster
    {
        public int Id { get; set; }
        public string PaymentName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
