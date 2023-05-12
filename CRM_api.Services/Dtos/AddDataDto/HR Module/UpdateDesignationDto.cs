namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class UpdateDesignationDto
    {
        public int DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
