using CRM_api.Services.Dtos.ResponseDto.Business_Module.LI_GI_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class AccountTransactionDto
    {
        public int Id { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocParticulars { get; set; }
        public InvestmentTypeDto? investmentType { get; set; }
        public string? DocType { get; set; }
        public string? DocNo { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? DocSubType { get; set; }
        public string? Narration { get; set; }
        public UserNameDto? UserMaster { get; set; }
        public AccountMasterDto? DebitAccount { get; set; }
        public AccountMasterDto? CreditAccount { get; set; }
        public int? Mgainid { get; set; }
        public CompanyMasterDto? CompanyMaster { get; set; }
        public string? TransactionType { get; set; }
        public PaymentTypeDto? TblPaymentType { get; set; }
        public MGainCurrancyDto? TblMgainCurrancyMaster { get; set; }
    }
}
