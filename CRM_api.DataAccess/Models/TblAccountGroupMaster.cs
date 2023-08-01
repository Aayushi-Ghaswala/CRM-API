using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblAccountGroupMaster
    {
        public int Id { get; set; }
        public int? RootGrpid { get; set; }
        public int? ParentGrpid { get; set; }
        public string? AccountGrpName { get; set; }
        public bool? Isdeleted { get; set; }

        [ForeignKey(nameof(ParentGrpid))]
        public virtual TblAccountGroupMaster? ParentGroup { get; set; }

        [ForeignKey(nameof(RootGrpid))]
        public virtual TblAccountGroupMaster? RootGroup { get; set; }

        public virtual ICollection<TblAccountMaster> AccountMasters { get; set; }

    }
}
