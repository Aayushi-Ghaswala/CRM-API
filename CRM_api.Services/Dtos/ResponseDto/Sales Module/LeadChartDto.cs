using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class LeadSummaryChartDto
    {
        public List<NewUserClientCountDto> NewUserClientCounts { get; set; }
        public List<LeadUserMFSummaryDto> LeadUserMFSummaryDtos { get; set; }

        public LeadSummaryChartDto(List<NewUserClientCountDto> newUserClientCounts, List<LeadUserMFSummaryDto> leadUserMFSummaryDtos)
        {
            NewUserClientCounts = newUserClientCounts;
            LeadUserMFSummaryDtos = leadUserMFSummaryDtos;
        }
    }
}
