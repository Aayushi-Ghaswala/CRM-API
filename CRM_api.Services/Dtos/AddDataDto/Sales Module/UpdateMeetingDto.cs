using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class UpdateMeetingDto
    {
        public int Id { get; set; }
        public int MeetingBy { get; set; }
        public string MeetingByName { get; set; }
        public int? LeadId { get; set; }
        public string Purpose { get; set; }
        public DateTime DateOfMeeting { get; set; }
        public Double Duration { get; set; }
        public string Mode { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
        public string? Link { get; set; }
        public bool IsCompleted { get; set; }
        public List<AddParticipantDto>? MeetingParticipants { get; set; }
        public List<AddAttachmentDto>? Files { get; set; }
    }
}