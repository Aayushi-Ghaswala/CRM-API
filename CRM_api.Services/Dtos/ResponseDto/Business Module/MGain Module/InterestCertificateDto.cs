namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class InterestCertificateDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public List<InterestReportDto> InterestReports { get; set; }
    }
}
