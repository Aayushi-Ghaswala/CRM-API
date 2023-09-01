namespace CRM_api.Services.Dtos.ResponseDto.Generic_Response
{
    public class HoldingChartReportDto
    {
        public string Month { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public decimal CurrentValue { get; set; }
    }
}
