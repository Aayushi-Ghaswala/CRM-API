namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class DesignationDto
    {
        public int DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }
        public DepartmentDto DepartmentMaster { get; set; }
    }
}
