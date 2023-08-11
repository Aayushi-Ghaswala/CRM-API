using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblEmployeeMaster
    {
        public int Id { get; set; }
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
        public DateTime? Dol { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual TblDepartmentMaster TblDepartmentMaster { get; set; }
        [ForeignKey(nameof(DesignationId))]
        public virtual TblDesignationMaster TblDesignationMaster { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual TblCityMaster TblCityMaster { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual TblStateMaster TblStateMaster { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual TblCountryMaster TblCountryMaster { get; set; }

        public virtual ICollection<TblEmployeeExperience> TblEmployeeExperiences { get; set; }
        public virtual ICollection<TblEmployeeQualification> TblEmployeeQualifications { get; set; }
    }
}
