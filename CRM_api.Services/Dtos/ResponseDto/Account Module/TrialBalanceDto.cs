namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class TrialBalanceDto
    {
        public string AccountGroupName { get; set; }
        public List<TrialBalanceTrasactionDto>? TrialBalanceTrasactions { get; set; }
        public decimal TotalDebit { get; set; } = 0;
        public decimal TotalCredit { get; set; } = 0;
    }
}
