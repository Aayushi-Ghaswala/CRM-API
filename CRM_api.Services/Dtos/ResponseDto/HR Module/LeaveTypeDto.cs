namespace CRM_api.Services.Dtos.ResponseDto.HR_Module
{
    public class LeaveTypeDto
    {
        public int LeaveId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? AllowedDay { get; set; }
        public bool? Isdeleted { get; set; } = false;
    }
}
