namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddMeetingParticipantDto
    {
        public int MeetingId { get; set; }
        public int? ParticipantId { get; set; }
        public int? LeadId { get; set; }
        public bool IsDeleted { get; set; }
    }
}