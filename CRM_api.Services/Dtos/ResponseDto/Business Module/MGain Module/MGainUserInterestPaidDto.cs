namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainUserInterestPaidDto
    {
        public string? UserName { get; set; }
        public decimal? totalInterestPaid { get; set; }
        public DateTime? DocDate { get; set; }
    }
}
