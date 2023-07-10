using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblMeetingMaster
    {
        public int Id { get; set; }
        public int MeetingBy { get; set; }
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
        public virtual TblUserMaster TblUserMaster { get; set; }
    }
}
