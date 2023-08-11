namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class EmployeeExperieneDto
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CompanyName { get; set; }
    }
}
