namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class AccountOpeningBalanceDto
    {
        public int Id { get; set; }
        public AccountMasterDto? TblAccountMaster { get; set; }
        public FinancialYearDto? TblFinancialYear { get; set; }
        public decimal? Balance { get; set; }
        public string? BalanceType { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
