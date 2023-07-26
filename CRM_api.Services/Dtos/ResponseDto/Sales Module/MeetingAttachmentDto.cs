using CRM_api.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class MeetingAttachmentDto
    {
        public int Id { get; set; }
        public MeetingDto TblMeetingMaster { get; set; }
        public string Attachment { get; set; }
        public bool IsDeleted { get; set; }
    }
}