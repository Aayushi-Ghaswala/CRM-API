namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class DisplayDepartmentDto
    {
        public List<DepartmentDto> Values { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
