namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class UpdateAccountOpeningBalanceDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public int? FinancialYearid { get; set; }
        public decimal? Balance { get; set; }
        public string? BalanceType { get; set; }
    }
}
