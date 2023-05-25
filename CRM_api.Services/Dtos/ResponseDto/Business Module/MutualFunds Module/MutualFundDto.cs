namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module
{
    public class MutualFundDto
    {
        public int trnid { get; set; }
        public string? Username { get; set; }
        public string? Transactiontype { get; set; }
        public DateTime? Date { get; set; }
        public string? Foliono { get; set; }
        public string? Schemename { get; set; }
        public double? Nav { get; set; }
        public decimal? Noofunit { get; set; }
        public decimal? Invamount { get; set; }
        public string? Tradeno { get; set; }
    }
}
