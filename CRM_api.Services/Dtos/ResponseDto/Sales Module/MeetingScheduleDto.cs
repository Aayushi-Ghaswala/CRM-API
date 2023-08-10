namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class MeetingScheduleDto
    {
        public string? WeekDay { get; set; }
        public int Meetings { get; set; } = 0;
        public int Calls { get; set; } = 0;
    }
}