namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class UpdateDepartmentDto
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
