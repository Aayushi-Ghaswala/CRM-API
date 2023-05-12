using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblDesignationMaster
    {
        public int DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool? Isdeleted { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual TblDepartmentMaster DepartmentMaster { get; set; }
    }
}
