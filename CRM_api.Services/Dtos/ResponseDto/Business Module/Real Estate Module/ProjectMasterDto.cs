using CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class ProjectMasterDto
    {
        public int Id { get; set; }
        public ProjectTypeDetailDto? TblProjectTypeDetail { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Taluko { get; set; }
        public string? City { get; set; }
        public CityMasterDto CityMaster { get; set; }
        public string? State { get; set; }
        public StateMasterDto StateMaster { get; set; }
        public string? Pincode { get; set; }
        public string? Remark { get; set; }
        public string? District { get; set; }
        public decimal? PricePerFoot { get; set; }
        public decimal? PricePerYard { get; set; }
        public decimal? TotalProperties { get; set; }
        public string? Description { get; set; }
        public string? Features { get; set; }
        public string? ProjectDocument { get; set; }
        public bool? IsActive { get; set; }
    }
}
