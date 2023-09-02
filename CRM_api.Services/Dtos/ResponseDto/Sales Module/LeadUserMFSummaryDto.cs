namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class LeadUserMFSummaryDto
    {
        public string Duration = null!;
        public decimal? CurrentValue = 0;
        public decimal? SIPAmount = 0;

        public LeadUserMFSummaryDto(string duration, decimal? currValue, decimal? sipAmount)
        {
            Duration = duration;
            CurrentValue = currValue;
            SIPAmount = sipAmount;
        }
    }
}
