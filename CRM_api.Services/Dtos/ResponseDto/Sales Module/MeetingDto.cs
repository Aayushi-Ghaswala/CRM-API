using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

using CRM_api.Services.Dtos.ResponseDto.User_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class MeetingDto
    {
        public int Id { get; set; }
        public UserNameDto TblUserMaster { get; set; }
        public LeadDto TblLeadMaster { get; set; }
        public string Purpose { get; set; }
        public DateTime DateOfMeeting { get; set; }
        public Double Duration { get; set; }
        public string Mode { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public string Link { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<MeetingParticipantDto> Participants { get; set; }
        public ICollection<MeetingAttachmentDto> Attachments { get; set; }
    }
}