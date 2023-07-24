using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblConversationHistoryMaster
    {
        public int Id { get; set; }
        public int? MeetingId { get; set; }
        public DateTime? Date { get; set; }
        public string? DiscussionSummary { get; set; }
        public DateTime? NextDate { get; set; }
        public string? Conclusion { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MeetingId))]
        public virtual TblMeetingMaster TblMeetingMaster { get; set; }
    }
}
