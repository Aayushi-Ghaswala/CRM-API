namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class UpdateAccountTransactionDto
    {
        public int Id { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocParticulars { get; set; }
        public string? DocType { get; set; }
        public string? DocNo { get; set; }
        public decimal? Debit { get; set; } = 0;
        public decimal? Credit { get; set; } = 0;
        public int? DocSubType { get; set; }
        public int? DocUserid { get; set; }
        public int? DebitAccountId { get; set; }
        public int? Mgainid { get; set; } = 0;
        public int? Companyid { get; set; }
        public int? CreditAccountId { get; set; }
        public string? TransactionType { get; set; }
        public int? Currencyid { get; set; }
        public string? Narration { get; set; }
    }
}
