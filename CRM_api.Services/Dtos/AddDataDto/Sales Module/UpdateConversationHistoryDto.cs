namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class UpdateConversationHistoryDto
    {
        public int Id { get; set; }
        public int? MeetingId { get; set; }
        public DateTime? Date { get; set; }
        public string? DiscussionSummary { get; set; }
        public DateTime? NextDate { get; set; }
        public string? Conclusion { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
