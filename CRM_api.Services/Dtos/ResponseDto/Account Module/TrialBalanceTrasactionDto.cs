namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class TrialBalanceTrasactionDto
    {
        public string AccountName { get; set; }
        public decimal Credit { get; set; } = 0;
        public decimal Debit { get; set; } = 0;
    }
}
