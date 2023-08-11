namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class UpdateEmployeeExperienceDto
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CompanyName { get; set; }
    }
}
