using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblMeetingMaster
    {
        public int Id { get; set; }
        public int? MeetingBy { get; set; }
        public int? LeadId { get; set; }
        public string Purpose { get; set; }
        public DateTime DateOfMeeting { get; set; }
        public Double Duration { get; set; }
        public string Mode { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
        public string? Link { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(MeetingBy))]
        public virtual TblUserMaster? TblUserMaster { get; set; }
        [ForeignKey(nameof(LeadId))]
        public virtual TblLeadMaster? TblLeadMaster { get; set; }

        public virtual ICollection<TblMeetingParticipant>? Participants { get; set; }
        public virtual ICollection<TblMeetingAttachment>? Attachments { get; set; }
    }
}