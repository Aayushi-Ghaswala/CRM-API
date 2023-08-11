using CRM_api.DataAccess.Models;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module
{
    public class FasttrackLedgerDto
    {
        public int FtId { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Credit { get; set; }
        public int? Debit { get; set; }
        public int? Userid { get; set; }
        public int? TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public string? Narration { get; set; }
        public virtual TblUserMaster? TblUserMaster { get; set; }
        public virtual TblInvesmentType? TblInvesmentType { get; set; }
        public virtual TblSubInvesmentType? TblSubInvesmentType { get; set; }
    }
}