namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainLedgerDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<InterestLedgerDto> InterestsLedger { get; set; }
        public decimal? Total { get; set; }
    }
}
