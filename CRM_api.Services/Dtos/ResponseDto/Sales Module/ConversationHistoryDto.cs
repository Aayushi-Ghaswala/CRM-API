namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class ConversationHistoryDto
    {
        public int Id { get; set; }
        public MeetingMasterDto TblMeetingMaster { get; set; }
        public DateTime? Date { get; set; }
        public string? DiscussionSummary { get; set; }
        public DateTime? NextDate { get; set; }
        public string? Conclusion { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
