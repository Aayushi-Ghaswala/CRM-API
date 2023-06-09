using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMgainPaymentMethod
    {
        public int Id { get; set; }
        public int? Mgainid { get; set; }
        public int? CurrancyId { get; set; }
        public string? PaymentMode { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? BankName { get; set; }
        public string? Ifsc { get; set; }
        public string? ReferenceNo { get; set; }
        public string? UpiTransactionNo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? UpiDate { get; set; }

        [ForeignKey(nameof(CurrancyId))]
        public virtual TblMgainCurrancyMaster TblMgainCurrancyMaster { get; set; }
        [ForeignKey(nameof(Mgainid))]
        public virtual TblMgaindetail TblMgaindetail { get; set; }
    }
}
