using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public class TblMeetingAttachment
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string Attachment { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(MeetingId))]
        public virtual TblMeetingMaster TblMeetingMaster { get; set; }
    }
}
