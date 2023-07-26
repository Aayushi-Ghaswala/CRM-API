namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class AccountMasterDto
    {
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public UserMasterDto? UserMaster { get; set; }
        public double? OpeningBalance { get; set; }
        public string? DebitCredit { get; set; }
        public DateTime? OpeningBalanceDate { get; set; }
        public AccountGroupDto? TblAccountGroupMaster { get; set; }
        public CompanyMasterDto? TblCompanyMaster { get; set; }
        public string? GstNo { get; set; }
        public DateTime? GstRegDate { get; set; }
        public string? AccountMobile { get; set; }
        public string? AccountEmail { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
