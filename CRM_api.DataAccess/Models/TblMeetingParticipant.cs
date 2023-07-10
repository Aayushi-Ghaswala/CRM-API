using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblMeetingParticipant
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public int ParticipantId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(MeetingId))]
        public virtual TblMeetingMaster TblMeetingMaster { get; set; }
        [ForeignKey(nameof(ParticipantId))]
        public virtual TblUserMaster TblUserMaster { get; set; }
    }
}
