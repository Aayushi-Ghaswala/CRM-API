namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
