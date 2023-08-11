namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class AddEmployeeDto
    {
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Doj { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? AadharNo { get; set; }
        public string? PanNo { get; set; }
        public List<AddEmployeeQualificationDto>? AddEmployeeQualification { get; set; }
        public List<AddEmployeeExperienceDto>? AddEmployeeExperiences { get; set; }
    }
}
