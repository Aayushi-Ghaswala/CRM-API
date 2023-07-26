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
    public class MeetingParticipantDto
    {
        public int Id { get; set; }
        public MeetingDto TblMeetingMaster { get; set; }
        public UserNameDto TblUserMaster { get; set; }
        public LeadDto TblLeadMaster { get; set; }
        public bool IsDeleted { get; set; }
    }
}