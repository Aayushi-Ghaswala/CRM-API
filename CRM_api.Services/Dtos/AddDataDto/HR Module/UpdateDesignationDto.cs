namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class UpdateDesignationDto
    {
        public int DesignationId { get; set; }
        public int? ParentDesignationId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
