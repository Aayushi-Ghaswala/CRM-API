namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module
{
    public class AddMGainPaymentDto
    {
        public int? Mgainid { get; set; }
        public int? CurrencyId { get; set; }
        public string? PaymentMode { get; set; }
        public string? ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string? BankName { get; set; }
        public string? Ifsc { get; set; }
        public string? ReferenceNo { get; set; }
        public string? UpiTransactionNo { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? UpiDate { get; set; }
    }
}
