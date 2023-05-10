using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblReferralMaster
    {
        public int RefId { get; set; }
        public string? RefName { get; set; }
        public string? RefMobileno { get; set; }
        public int? RefParentid { get; set; }
        public DateTime? RefDate { get; set; }

        public virtual TblUserMaster? RefParent { get; set; }
    }
}
