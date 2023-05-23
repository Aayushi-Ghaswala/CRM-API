namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module
{
    public class AddMutualfundsDto
    {
        public string? Username { get; set; }
        public string? Userpan { get; set; }
        public string? Transactiontype { get; set; }
        public DateTime? Date { get; set; }
        public string? Foliono { get; set; }
        public int? SchemeId { get; set; }
        public string? Schemename { get; set; }
        public int? Userid { get; set; }
        public double? Nav { get; set; }
        public decimal? Noofunit { get; set; }
        public decimal? Invamount { get; set; }
        public decimal? Unitbalance { get; set; }
        public string? Tradeno { get; set; }
    }
}
