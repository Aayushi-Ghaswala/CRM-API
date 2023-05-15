namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class AddLeaveTypeDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? AllowedDay { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
