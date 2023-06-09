﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRoleAssignment
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual TblRoleMaster TblRoleMaster { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual TblUserMaster TblUserMaster { get; set; }

    }
}
