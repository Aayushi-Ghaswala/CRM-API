using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFasttrackLedger
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
    }
}
