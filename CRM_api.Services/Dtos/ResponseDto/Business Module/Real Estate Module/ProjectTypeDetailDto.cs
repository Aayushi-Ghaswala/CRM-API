namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Real_Estate_Module
{
    public class ProjectTypeDetailDto
    {
        public int Id { get; set; }
        public ProjectTypeDto? TblProjectTypeMaster { get; set; }
        public string? ProjectTypeDetail { get; set; }
    }
}
