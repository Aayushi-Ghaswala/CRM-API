using CRM_api.DataAccess.Models;

namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class EmployeeMasterDto
    {
        public int Id { get; set; }
        public DepartmentDto? TblDepartmentMaster { get; set; }
        public DesignationDto? TblDesignationMaster { get; set; }
        public CityMasterDto? TblCityMaster { get; set; }
        public StateMasterDto? TblStateMaster { get; set; }
        public CountryMasterDto? TblCountryMaster { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? Doj { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? AadharNo { get; set; }
        public string? PanNo { get; set; }
        public DateTime? Dol { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<EmployeeExperieneDto> TblEmployeeExperiences { get; set; }
        public virtual ICollection<EmployeeQualificationDto> TblEmployeeQualifications { get; set; }
    }
}
