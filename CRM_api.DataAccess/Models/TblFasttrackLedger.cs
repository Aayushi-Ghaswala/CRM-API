﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFasttrackLedger
    {
        public int FtId { get; set; }
        public DateTime? Timestamp { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public int? Userid { get; set; }
        public int? TypeId { get; set; }
        public int? SubTypeId { get; set; }
        public string? Narration { get; set; }

        [ForeignKey(nameof(Userid))]
        public virtual TblUserMaster? TblUserMaster { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual TblInvesmentType? TblInvesmentType { get; set; }
        [ForeignKey(nameof(SubTypeId))]
        public virtual TblSubInvesmentType? TblSubInvesmentType { get; set; }
    }
}
