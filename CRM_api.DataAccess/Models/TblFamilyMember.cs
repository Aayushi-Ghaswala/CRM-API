using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFamilyMember
    {
        public int Memberid { get; set; }
        public string? Name { get; set; }
        public string Mobileno { get; set; } = null!;
        public string? Relation { get; set; }
        public int? Userid { get; set; }
        public int? Familyid { get; set; }
        public int? RelativeUserId { get; set; }
    }
}
