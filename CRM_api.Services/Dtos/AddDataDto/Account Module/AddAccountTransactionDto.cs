namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class AddAccountTransactionDto
    {
        public DateTime? DocDate { get; set; }
        public string? DocParticulars { get; set; }
        public string? DocType { get; set; }
        public string? DocNo { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? DocSubType { get; set; }
        public int? DocUserid { get; set; }
        public int? DebitAccountId { get; set; }
        public int? Mgainid { get; set; }
        public int? Companyid { get; set; }
        public int? CreditAccountId { get; set; }
    }
}
