namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class UpdateMeetingAttachmentDto
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int Attachment { get; set; }
        public bool IsDeleted { get; set; }
    }
}