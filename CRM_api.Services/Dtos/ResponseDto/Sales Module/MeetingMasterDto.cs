using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class MeetingMasterDto
    {
        public int Id { get; set; }
        public TblUserMaster TblUserMaster { get; set; }
        public string? Purpose { get; set; }
        public DateTime? DateOfMeeting { get; set; }
        public double? Duration { get; set; }
        public string? Mode { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
        public string? Link { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
