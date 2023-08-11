namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class UpdateEmployeeQualificationDto
    {
        public int Id { get; set; }
        public int? EmpId { get; set; }
        public string? Year { get; set; }
        public string? Degree { get; set; }
        public decimal? Score { get; set; }
        public string? UniCollege { get; set; }
    }
}
