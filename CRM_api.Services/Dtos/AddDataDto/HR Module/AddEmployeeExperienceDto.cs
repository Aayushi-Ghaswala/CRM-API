namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class AddEmployeeExperienceDto
    {
        public string? JobTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CompanyName { get; set; }
    }
}
