namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddConversationHistoryDto
    {
        public int? MeetingId { get; set; }
        public string? DiscussionSummary { get; set; }
        public DateTime? NextDate { get; set; }
        public string? Conclusion { get; set; }
    }
}
