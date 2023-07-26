namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class AddUserAccountDto
    {
        public string? AccountName { get; set; }
        public int? UserId { get; set; }
        public double? OpeningBalance { get; set; }
        public string? DebitCredit { get; set; }
        public DateTime? OpeningBalanceDate { get; set; }
        public int? AccountGrpid { get; set; }
        public int? Companyid { get; set; }
        public string? GstNo { get; set; }
        public DateTime? GstRegDate { get; set; }
        public string? AccountMobile { get; set; }
        public string? AccountEmail { get; set; }
    }
}
